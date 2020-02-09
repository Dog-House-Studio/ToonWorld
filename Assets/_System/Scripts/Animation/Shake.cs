using UnityEngine;

namespace DogHouse.ToonWorld.Animation
{
    /// <summary>
    /// Shake is a script that when told
    /// to, will shake this current gameobject's
    /// position. This should be attached to an
    /// object, who's local position at rest
    /// is (0,0,0).
    /// </summary>
    public class Shake : MonoBehaviour
    {
        #region Private Variables
        [SerializeField]
        [Range(0f, 10f)]
        private float m_shakeAmount;

        [SerializeField]
        [Range(0f, 10f)]
        private float m_shakeDampening;

        [SerializeField]
        private bool m_shakeX;

        [SerializeField]
        private bool m_shakeY;

        [SerializeField]
        private bool m_shakeZ;

        [MethodButton("AddShake")]
        [SerializeField]
        private bool editorFoldout;

        private bool m_shaking = false;
        private float m_currentShakeAmount = 0f;
        private Vector3 m_heading = Vector3.zero;
        private Vector3 m_currentPosition;
        #endregion

        #region Main Methods
        public void AddShake()
        {
            m_shaking = true;

            //m_currentShakeAmount += Random.Range(0.0001f, m_shakeAmount);
            //m_currentShakeAmount = Mathf.Min(m_currentShakeAmount, m_shakeAmount);

            m_currentShakeAmount = m_shakeAmount;
        }

        private void Update()
        {
            if (!m_shaking) return;

            if (Mathf.Approximately(m_currentShakeAmount, 0f))
            {
                m_shaking = false;
            }

            m_heading = Random.insideUnitSphere * m_currentShakeAmount;
            if (!m_shakeX) m_heading.x = 0f;
            if (!m_shakeY) m_heading.y = 0f;
            if (!m_shakeZ) m_heading.z = 0f;

            m_currentPosition = Vector3.Lerp(Vector3.zero, m_heading, 0.5f);
            transform.localPosition = m_currentPosition;

            m_currentShakeAmount -= m_shakeDampening * Time.deltaTime;
            m_currentShakeAmount = Mathf.Max(m_currentShakeAmount, 0f);
        }
        #endregion
    }
}
