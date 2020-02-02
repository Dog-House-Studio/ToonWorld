using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DogScaffold;

namespace DogHouse.ToonWorld.Services
{
    /// <summary>
    /// BasicMusicService is a script that implements
    /// the IMusicService. The Music Service is 
    /// responsible for playing music.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class BasicMusicService : BaseService<IMusicService>,
        IMusicService
    {
        #region Private Variables
        [SerializeField]
        private AudioClip[] m_musicTracks;

        private AudioSource m_source;
        private bool m_isActive = false;
        #endregion

        #region Main Methods
        public void Start()
        {
            m_source = GetComponent<AudioSource>();
        }

        public void Play()
        {
            m_isActive = true;
            if (m_source.isPlaying) return;
            PlayNewAudioTrack();
        }

        public void Stop()
        {
            m_isActive = false;
            if(m_source.isPlaying)
            {
                m_source.Stop();
            }
        }

        void Update()
        {
            if(m_isActive && !m_source.isPlaying)
            {
                PlayNewAudioTrack();
            }
        }
        #endregion

        #region Utility Methods
        private void PlayNewAudioTrack()
        {
            m_source.clip = m_musicTracks[Random.Range(0, m_musicTracks.Length)];
            m_source.Play();
        }
        #endregion
    }
}
