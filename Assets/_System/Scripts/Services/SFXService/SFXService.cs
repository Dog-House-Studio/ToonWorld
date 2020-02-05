using System.Collections.Generic;
using UnityEngine;
using DogScaffold;
using UnityEngine.Audio;
using System.Linq;

namespace DogHouse.ToonWorld.Services
{
    /// <summary>
    /// SFXService is an implementation of 
    /// the sfx interface. The SFXService is
    /// responsible for playing sfx for other
    /// scripts.
    /// </summary>
    public class SFXService : BaseService<ISFXService>, ISFXService
    {
        #region Private Variables
        [SerializeField]
        private AudioMixerGroup[] m_groups;

        private List<AudioSource> m_sources 
            = new List<AudioSource>();
        #endregion

        #region Main Methods
        private void Start()
        {
            GenerateSources();
        }

        public void PlaySFX(AudioClip clip, AudioMixerGroup output)
        {
            if (!m_groups.Contains(output)) return;

            int index = FindIndex(output);
            m_sources[index].PlayOneShot(clip);
        }
        #endregion

        #region Utility Methods
        private void GenerateSources()
        {
            for(int i = 0; i < m_groups.Length; i++)
            {
                CreateSource(m_groups[i]);
            }
        }

        private void CreateSource(AudioMixerGroup audioMixerGroup)
        {
            GameObject child = Instantiate(new GameObject());
            child.transform.SetParent(transform);
            AudioSource source = child.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.outputAudioMixerGroup = audioMixerGroup;
            m_sources.Add(source);
        }

        private int FindIndex(AudioMixerGroup output)
        {
            for (int i = 0; i < m_groups.Length; i++)
            {
                if (m_groups[i] == output)
                {
                    return i;
                }
            }

            return -1;
        }
        #endregion
    }
}
