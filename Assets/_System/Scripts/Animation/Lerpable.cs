using UnityEngine;
using System;

namespace DogHouse.ToonWorld.Animation
{
    /// <summary>
    /// Lerpable is a script that lerps some
    /// value. The value that is lerped can be
    /// used for animation purposes.
    /// </summary>
    [Serializable]
    public class Lerpable
    {
        #region Public Variables
        public float LerpValue => m_lerpValue;
        public bool IsLerping => m_isLerping;
        #endregion

        #region Private Variables
        [SerializeField]
        private AnimationCurve m_lerpCurve;

        [SerializeField]
        [Range(0f,1f)]
        private float m_lerpTime;

        private float m_timePassed;
        private float m_lerpValue;
        private bool m_isLerping;
        #endregion

        #region Main Methods
        public void BeginLerping()
        {
            m_isLerping = true;
            m_timePassed = 0f;
        }

        public void Update()
        {
            if (!m_isLerping) return;

            m_timePassed += Time.deltaTime;
            m_timePassed = Mathf.Clamp(m_timePassed, 0f, m_lerpTime);
            m_lerpValue = m_lerpCurve.Evaluate(m_timePassed / m_lerpTime);

            if(Mathf.Approximately(m_lerpTime, m_timePassed))
            {
                m_isLerping = false;
            }
        }
        #endregion
    }
}
