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
        private AnimationCurve m_whiteCurve;

        [SerializeField]
        private AnimationCurve m_colorCurve;

        [SerializeField]
        private float m_scaleAmount;

        private bool m_animating = false;
        private float m_lerp = 0f;
        private float m_timePassed = 0f;
        private bool m_filling = false;
        private Color m_color;
        private Vector3 m_localScale;
        #endregion

        #region Main Methods
        private void Start()
        {
            m_localScale = transform.localScale;
        }

        public void SetFillColor(Color col)
        {
            m_color = col;
            col.a = 0f;
            m_fillImage.color = col;
        }

        public void SetFilled(bool value)
        {
            m_animating = true;
            m_filling = value;
            m_timePassed = 0f;
            m_lerp = 0f;
        }

        public void ResetBar()
        {
            Color c = m_fillImage.color;
            c.a = 0f;
            m_fillImage.color = c;
            m_animating = false;
        }

        private void Update()
        {
            if (!m_animating) return;
            float passedValue = Mathf.Clamp01(m_timePassed / m_transitionTime);

            if (Mathf.Approximately(passedValue, 1f))
            {
                m_animating = false;
            }

            m_timePassed += Time.deltaTime;
            Vector3 scale = m_localScale;

            if (passedValue < 0.5f)
            {
                m_lerp = m_whiteCurve.Evaluate(passedValue * 2f);
                Color c = Color.white;
                c.a = (m_filling) ? m_lerp : 1 - m_lerp;
                m_fillImage.color = c;

                
                scale.y = Mathf.Lerp(1f,m_localScale.y * m_scaleAmount, m_lerp);
                this.transform.localScale = scale;
                return;
            }

            m_lerp = m_colorCurve.Evaluate((passedValue  - 0.5f) * 2f);
            m_fillImage.color = Color.Lerp(Color.white, m_color, m_lerp);

            scale.y = Mathf.Lerp(m_localScale.y * m_scaleAmount, 1f, m_lerp);
            this.transform.localScale = scale;
        }
        #endregion
    }
}
