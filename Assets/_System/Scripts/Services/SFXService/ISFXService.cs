using UnityEngine;
using DogScaffold;
using UnityEngine.Audio;

namespace DogHouse.ToonWorld.Services
{
    /// <summary>
    /// ISFXService is an interface that describes
    /// a service that can be used to play any sfx.
    /// </summary>
    public interface ISFXService : IService
    {
        void PlaySFX(AudioClip clip, AudioMixerGroup output);
    }
}
