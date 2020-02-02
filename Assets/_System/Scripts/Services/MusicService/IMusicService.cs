using DogScaffold;

namespace DogHouse.ToonWorld.Services
{
    /// <summary>
    /// The IMusicService is an interface for a
    /// service that plays music and changes 
    /// music when the current track is finished.
    /// 
    /// The music service is able to stop and 
    /// restart when told to.
    /// </summary>
    public interface IMusicService : IService
    {
        void Play();
        void Stop();
    }
}
