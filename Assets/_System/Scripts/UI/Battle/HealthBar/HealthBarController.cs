﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DogScaffold;
using DogHouse.CoreServices;
using DogHouse.ToonWorld.Animation;
using UnityEngine.Animations;
using DogHouse.ToonWorld.UI;
using static UnityEngine.Mathf;

namespace DogHouse.ToonWorld.CombatControllers
{
    /// <summary>
    /// HealthBarController is a script that controls
    /// the visual aspects of the health bar for a 
    /// unit on the battlefield.
    /// </summary>
    public class HealthBarController : MonoBehaviour, IUnitIdentifier
    {
        #region Public Variables
        public float FadeValue => m_canvasGroup.alpha;
        #endregion

        #region Private Variables
        [Header("Elements")]
        [SerializeField]
        private CanvasGroup m_canvasGroup;

        [SerializeField]
        private Image m_classEmblemImage;

        [SerializeField]
        private TMP_Text m_nameText;

        [SerializeField]
        private TMP_Text m_classNameText;

        [SerializeField]
        private TMP_Text m_unitLevelText;

        [SerializeField]
        private TMP_Text m_healthText;

        [SerializeField]
        private Slider m_healthSlider;

        [SerializeField]
        private LookAtConstraint m_lookAtConstraint;

        [SerializeField]
        private Shake m_shake;

        [SerializeField]
        private Image m_healthBarImage;

        [SerializeField]
        private GameObject m_numberEffectPrefab;

        [SerializeField]
        private HealthBarHealEffectController m_healController;

        [Header("Audio")]
        [SerializeField]
        private AudioSource m_source;

        [SerializeField]
        private AudioClip m_hurtSFX;

        [SerializeField]
        private AudioClip m_healSFX;

        [SerializeField]
        [Range(0f,2f)]
        private float m_pitchMax;

        [SerializeField]
        [Range(0f, 2f)]
        private float m_pitchMin;

        [Header("Lerping")]
        [SerializeField]
        [Range(0.0001f, 10f)]
        private float m_lerpValue;

        [SerializeField]
        private Color m_fullColor;

        [SerializeField]
        private Color m_emptyColor;

        private float m_lazyBarValue = 1f;
        private float m_barValue = 1f;

        private GameUnitDefinition m_definition;

        private ServiceReference<ICameraFinder> m_cameraService 
            = new ServiceReference<ICameraFinder>();
        #endregion

        #region Main Methods
        void Start()
        {
            m_cameraService.AddRegistrationHandle(OnCameraServiceRegistered);
        }

        void OnDestroy()
        {
            if(m_definition != null)
            {
                m_definition.OnStatsChanged -= HandleStatChange;
            }
        }

        public void SetDataDisplay(GameUnitDefinition definition)
        {
            m_classEmblemImage.overrideSprite = definition.BaseClassType.ClassSprite;
            m_classNameText.text = definition.BaseClassType.ClassName;
            m_nameText.text = definition.UnitName;
            m_unitLevelText.text = definition.Level.ToString();
            SetHealthText(definition.Stats, definition.BaseStats);

            m_definition = definition;
            m_definition.OnStatsChanged -= HandleStatChange;
            m_definition.OnStatsChanged += HandleStatChange;
        }

        public void SetFadeValue(float value)
        {
            m_canvasGroup.alpha = Clamp01(value);
        }

        void Update()
        {
            if (Approximately(m_barValue, m_lazyBarValue)) return;

            m_lazyBarValue = Lerp(m_lazyBarValue, m_barValue, m_lerpValue * Time.deltaTime);
            m_healthSlider.value = m_lazyBarValue;

            m_healthBarImage.color = Color.Lerp(m_emptyColor, m_fullColor, m_lazyBarValue);
        }
        #endregion

        #region Utility Methods
        void OnCameraServiceRegistered()
        {
            ConstraintSource source = new ConstraintSource();
            source.weight = 1f;
            source.sourceTransform = m_cameraService.Reference.Camera.transform;
            m_lookAtConstraint.AddSource(source);
        }

        private void SetHealthText(UnitStats currentStats, UnitStats baseStats)
        {
            string healthTextValue = "HP " + currentStats.Health.ToString() 
                + " / " + baseStats.Health.ToString();

            m_healthText.text = healthTextValue;

            m_barValue = ((float)currentStats.Health) / ((float)baseStats.Health);
        }

        private void HandleStatChange(UnitStats currentStats, UnitStats baseStats, int delta)
        { 
            SetHealthText(currentStats, baseStats);
            CreateNumberEffect(delta);
            m_source.pitch = Lerp(m_pitchMin, m_pitchMax, m_barValue);

            if (delta < 0)
            {
                m_shake.AddShake();
                m_source?.PlayOneShot(m_hurtSFX);
            }

            if(delta > 0)
            {
                m_healController?.PlayEffect();
                m_source.PlayOneShot(m_healSFX);
            }
        }

        private void CreateNumberEffect(int delta)
        {
            GameObject numberEffectObject = Instantiate(m_numberEffectPrefab);
            numberEffectObject.transform.SetParent(this.transform);
            numberEffectObject.transform.localPosition = Vector3.forward * 0.05f;
            numberEffectObject.transform.localRotation = Quaternion.identity;

            NumberEffectController controller = numberEffectObject.GetComponent<NumberEffectController>();
            controller?.SetValue(delta);
        }
        #endregion
    }
}
