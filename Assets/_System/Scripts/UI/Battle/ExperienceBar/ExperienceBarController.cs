using UnityEngine;
using TMPro;
using DogHouse.ToonWorld.Animation;
using System.Collections.Generic;
using DogHouse.ToonWorld.UI;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Animations;
using DogScaffold;
using DogHouse.CoreServices;
using static UnityEngine.Mathf;

namespace DogHouse.ToonWorld.CombatControllers
{
    /// <summary>
    /// ExperienceBarController is a script that controls the
    /// experience bar visuals.
    /// </summary>
    public class ExperienceBarController : MonoBehaviour, IUnitIdentifier
    {
        #region Private Variables
        [Header("General")]
        [SerializeField]
        private int m_level;

        [Header("Elements")]
        [SerializeField]
        private TMP_Text m_percentageText;

        [SerializeField]
        private GameObject m_barParent;

        [SerializeField]
        private Image m_classEmblem;

        [SerializeField]
        private TMP_Text m_levelText;

        [SerializeField]
        private TMP_Text m_overlayLevelText;

        [SerializeField]
        private GameObject m_barPrefab;

        [SerializeField]
        private AudioSource m_audioSource;

        [SerializeField]
        private LookAtConstraint m_lookAtContraint;

        [SerializeField]
        private PlayableDirector m_playableDirector;

        [SerializeField]
        private ExperienceBarOverlayController m_overlayController;

        [SerializeField]
        private TMP_Text m_nameText;

        [SerializeField]
        private TMP_Text m_classNameText;

        [Header("Lerping")]
        [SerializeField]
        private AnimationCurve m_lerpCurve;

        [SerializeField]
        [Range(0.0001f, 10f)]
        private float m_lerpTime;
        private float m_currentTime;

        [Header("Value")]
        [Range(0f, 1f)]
        [SerializeField]
        private float m_progressAmount;
        private float m_slowAmount = 0f;
        private float m_startSlowAmount = 0f;

        [Header("Shake")]
        [SerializeField]
        [Range(0f, 1f)]
        private float m_shakeThreshhold;

        [SerializeField]
        private Shake m_textShaker;

        [Header("Slots")]
        [SerializeField]
        private int m_slotAmount;

        [SerializeField]
        private float m_barOffsetAmount;

        [SerializeField]
        private float m_expSlotZRotOffset;

        [Header("Color")]
        [SerializeField]
        private Color m_startColor;

        [SerializeField]
        private Color m_endColor;

        [Header("Audio")]
        [SerializeField]
        private AudioClip m_barAddedClip;

        [SerializeField]
        [Range(1f, 2f)]
        private float m_maxPitch;

        private int m_currentBarIndex = 0;
        private bool m_animating = false;
        private float m_barThreshholdAmount;
        private List<ExperienceBarSlotController> m_barControllers 
            = new List<ExperienceBarSlotController>();

        private ServiceReference<ICameraFinder> m_cameraFinderService 
            = new ServiceReference<ICameraFinder>();

        [MethodButton("TestFull")]
        [SerializeField]
        private bool editorFoldout;
        #endregion

        #region Main Methods
        private void Start()
        {
            m_barThreshholdAmount = 1f / ((float)m_slotAmount);
            Vector3 offset = Vector3.zero;
            for(int i = 0; i < m_slotAmount; i++)
            {
                float lerp = ((float)i) / ((float)m_slotAmount - 1);
                CreateExpSlot(offset, lerp);
                offset.x += m_barOffsetAmount;
            }

            m_classEmblem.color = m_startColor;

            m_levelText.text = m_level.ToString();
            m_overlayLevelText.text = m_level.ToString();

            m_overlayController.OnValueChanged -= HandleLevelValueChanged;
            m_overlayController.OnValueChanged += HandleLevelValueChanged;

            m_cameraFinderService.AddRegistrationHandle(HandleCameraFinderAvailable);
        }

        public void SetValue(float value)
        {
            m_progressAmount = Clamp01(value);
            m_animating = true;
            m_startSlowAmount = m_slowAmount;
        }

        private void Update()
        {
            if (!m_animating) return;

            m_currentTime += Time.deltaTime;
            float lerp = Clamp01(m_currentTime / m_lerpTime);
            lerp = m_lerpCurve.Evaluate(lerp);

            m_slowAmount = Lerp(m_startSlowAmount, m_progressAmount, lerp);

            Color lerpedColor = Color.Lerp(m_startColor, m_endColor, m_slowAmount);
            m_classEmblem.color = lerpedColor;

            SetText(m_slowAmount);

            bool playSFX = false;

            while (m_currentBarIndex * m_barThreshholdAmount < m_slowAmount)
            {
                m_barControllers[m_currentBarIndex].SetFilled(true);
                m_currentBarIndex++;
                playSFX = true;
            } 

            if(playSFX)
            {
                m_audioSource.pitch = Lerp(1f, m_maxPitch, m_slowAmount);
                m_audioSource.PlayOneShot(m_barAddedClip);
            }

            if (Random.Range(lerp, 1f) > m_shakeThreshhold)
            {
                m_textShaker.AddShake();
            }

            if(Approximately(lerp, 1f))
            {
                m_animating = false;
                if(Mathf.Approximately(m_progressAmount, 1f))
                {
                    HandleLevelUp();
                }
            }
        }

        private void SetText(float value)
        {
            value = value * 100f;
            string text = value.ToString("0");
            m_percentageText.text = text + "%";
        }

        public void SetDataDisplay(GameUnitDefinition definition)
        {
            m_nameText.text = definition.UnitName;
            m_classNameText.text = definition.BaseClassType.ClassName;
            m_classEmblem.overrideSprite = definition.BaseClassType.ClassSprite;
            m_level = definition.Level;
        }
        #endregion

        #region Utility Methods
        private void CreateExpSlot(Vector3 offset, float lerpValue)
        {
            GameObject bar = Instantiate(m_barPrefab);
            bar.transform.SetParent(m_barParent.transform);
            bar.transform.localPosition = offset;
            bar.transform.localRotation = Quaternion.Euler(0f, 0f, m_expSlotZRotOffset);

            ExperienceBarSlotController controller = bar.GetComponent<ExperienceBarSlotController>();
            m_barControllers.Add(controller);
            controller.SetFillColor(Color.Lerp(m_startColor, m_endColor, lerpValue));
        }

        private void HandleLevelUp()
        {
            m_overlayController?.Play();
            m_playableDirector?.Play();
        }

        private void HandleLevelValueChanged()
        {
            m_level++;
            m_levelText.text = m_level.ToString();
            m_overlayLevelText.text = m_level.ToString();

            m_progressAmount = 0f;
            m_slowAmount = 0f;
            m_classEmblem.color = m_startColor;
            SetText(0f);
            m_currentTime = 0f;
            m_currentBarIndex = 0;

            foreach (ExperienceBarSlotController controller in m_barControllers)
            {
                controller?.ResetBar();
            }
        }

        private void HandleCameraFinderAvailable()
        {
            ConstraintSource source = new ConstraintSource();
            source.weight = 1f;
            source.sourceTransform = m_cameraFinderService.Reference.Camera.transform;
            m_lookAtContraint.AddSource(source);
        }
        #endregion

        #region Editor Functions
        private void TestFull()
        {
            SetValue(1f);
        }
        #endregion
    }
}
