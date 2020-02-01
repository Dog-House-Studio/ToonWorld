using DogScaffold;
using System;

namespace DogHouse.ToonWorld.Services
{
    /// <summary>
    /// ILoadingScreenService is a script that defines what
    /// a loading screen service must do. A loading screen
    /// service must give access to other objects to request
    /// a loading screen.
    /// </summary>
    public interface ILoadingScreenService : IService
    {
        void TransitionIn(Action callback = null);
        void TransitionOut(Action callback = null);
    }
}
