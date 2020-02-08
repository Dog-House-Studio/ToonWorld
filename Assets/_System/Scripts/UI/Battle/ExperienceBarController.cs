using UnityEngine;
using TMPro;
using System;

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
        private float m_lerpScalar;

        [SerializeField]
        [Range(0.0001f, 10f)]
        private float m_lerpTime;
        private float m_currentTime;

        [Header("Value")]
        [Range(0f, 1f)]
        [SerializeField]
        private float m_progressAmount;
        private float m_slowAmount = 0f;
        #endregion

        #region Main Methods
        private void Update()
        {
            if(Mathf.Approximately(m_slowAmount, m_progressAmount))
            {
                m_currentTime = 0f;
                return;
            }

            m_currentTime += Time.deltaTime;
            float lerp = Mathf.Clamp01(m_currentTime / m_lerpTime);
            lerp = m_lerpCurve.Evaluate(lerp);

            m_slowAmount = Mathf.Lerp(m_slowAmount, m_progressAmount, lerp * m_lerpScalar * Time.deltaTime);
            SetText(m_slowAmount);
        }

        private void SetText(float value)
        {
            value = value * 100f;
            string text = value.ToString("0");
            m_percentageText.text = text + "%";
        }
        #endregion
    }
}
