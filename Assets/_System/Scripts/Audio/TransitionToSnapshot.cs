using UnityEngine;
using UnityEngine.Audio;

namespace DogHouse.ToonWorld.Audio
{
    /// <summary>
    /// TransitionToSnapshot is a script that transitions
    /// the current audio snapshot to a given one.
    /// </summary>
    public class TransitionToSnapshot : MonoBehaviour
    {
        #region Private Variables
        [SerializeField]
        private AudioMixerSnapshot m_snapshot;

        [SerializeField]
        private float m_transitionTime;
        #endregion

        #region Main Methods
        public void Transition()
        {
            m_snapshot?.TransitionTo(m_transitionTime);
        }
        #endregion
    }
}
