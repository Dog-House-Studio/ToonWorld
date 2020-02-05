using UnityEngine;
using DogScaffold;
using UnityEngine.Audio;

namespace DogHouse.ToonWorld.Services
{
    /// <summary>
    /// SFXRequest is a component that can 
    /// be attached to a gameobject to access
    /// the sfx service.
    /// </summary>
    public class SFXRequest : MonoBehaviour
    {
        #region Private Variables
        [SerializeField]
        private AudioClip m_clip;

        [SerializeField]
        private AudioMixerGroup m_group;

        private ServiceReference<ISFXService> m_sfxService 
            = new ServiceReference<ISFXService>();
        #endregion

        #region Main Methods
        public void RequestSFX()
        {
            m_sfxService.Reference?.PlaySFX(m_clip, m_group);
        }
        #endregion
    }
}
