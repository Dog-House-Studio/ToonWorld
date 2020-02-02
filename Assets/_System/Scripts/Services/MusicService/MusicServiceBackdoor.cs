using UnityEngine;
using DogScaffold;

namespace DogHouse.ToonWorld.Services
{
    /// <summary>
    /// MusicServiceBackdoor is a script that
    /// can be used by other objects to interface
    /// with the music service.
    /// </summary>
    public class MusicServiceBackdoor : MonoBehaviour, IMusicService
    {
        #region Private Variables
        private ServiceReference<IMusicService> m_musicService 
            = new ServiceReference<IMusicService>();
        #endregion

        #region Main Methods
        public void Play()
        {
            m_musicService.Reference?.Play();
        }

        public void RegisterService()
        {
        }

        public void Stop()
        {
            m_musicService.Reference?.Stop();
        }
        #endregion
    }
}
