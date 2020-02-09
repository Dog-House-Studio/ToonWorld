using UnityEngine;
using TMPro;
using DogHouse.ToonWorld.Animation;
using static UnityEngine.Mathf;

namespace DogHouse.ToonWorld.CombatControllers
{
    /// <summary>
    /// ExperienceBarController is a script that controls the
    /// experience bar visuals.
    /// </summary>
    public class ExperienceBarController : MonoBehaviour
    {
        #region Private Variables
        [Header("Elements")]
        [SerializeField]
        private TMP_Text m_percentageText;

        [SerializeField]
        private TMP_Text m_levelUpText;

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

        private bool m_animating = false;

        [MethodButton("TestFull")]
        [SerializeField]
        private bool editorFoldout;
        #endregion

        #region Main Methods
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
            SetText(m_slowAmount);

            if (Random.Range(lerp, 1f) > m_shakeThreshhold)
            {
                m_textShaker.AddShake();
            }

            if(Approximately(lerp, 1f))
            {
                m_animating = false;
            }
        }

        private void SetText(float value)
        {
            value = value * 100f;
            string text = value.ToString("0");
            m_percentageText.text = text + "%";
        }
        #endregion

        #region Utility Methods

        #endregion

        #region Editor Functions
        private void TestFull()
        {
            SetValue(1f);
        }

        private void TestEmpty()
        {
            SetValue(0f);
        }
        #endregion
    }
}
