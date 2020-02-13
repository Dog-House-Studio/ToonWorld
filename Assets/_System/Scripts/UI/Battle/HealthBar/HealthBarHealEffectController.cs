using UnityEngine;

namespace DogHouse.ToonWorld.UI
{
    /// <summary>
    /// HealthBarHealEffectController is a script that controls
    /// the visual aspect of the heal effect.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class HealthBarHealEffectController : MonoBehaviour
    {
        #region Private Variables
        private Animator m_animator;
        private const string TRIGGER_NAME = "Play";
        #endregion

        #region Main Methods
        public void PlayEffect()
        {
            m_animator?.SetTrigger(TRIGGER_NAME);
        }

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }
        #endregion
    }
}
