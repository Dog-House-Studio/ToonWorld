using UnityEngine;
using UnityEngine.UI;

namespace DogHouse.ToonWorld.UI
{
    /// <summary>
    /// ExperienceBarSlotController is a script that controls
    /// the visual of a exp bar slot.
    /// </summary>
    public class ExperienceBarSlotController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField]
        private Image m_fillImage;

        [SerializeField]
        [Range(0.0001f, 1f)]
        private float m_transitionTime;

        [SerializeField]
        private AnimationCurve m_lerpCurve;

        [SerializeField]
        private float m_scaleAmount;

        private bool m_animating = false;
        private float m_lerp = 0f;
        private float m_timePassed = 0f;
        private bool m_filling = false;
        private Color m_color;
        #endregion

        #region Main Methods
        public void SetFillColor(Color col)
        {
            col.a = m_fillImage.color.a;
            m_fillImage.color = col;
            m_color = col;
        }

        public void SetFilled(bool value)
        {
            m_animating = true;
            m_filling = value;
            m_timePassed = 0f;
            m_lerp = 0f;
        }

        private void Update()
        {
            if (!m_animating) return;
            if(Mathf.Approximately(m_lerp, 1f))
            {
                m_animating = false;
            }

            m_timePassed += Time.deltaTime;
            m_lerp = m_lerpCurve.Evaluate(Mathf.Clamp01(m_timePassed / m_transitionTime));

            m_color.a = (m_filling) ? m_lerp : 1 - m_lerp;
            m_fillImage.color = m_color;
        }
        #endregion
    }
}
