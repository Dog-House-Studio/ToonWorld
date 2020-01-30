using System;

namespace DogHouse.ToonWorld.UI
{
    /// <summary>
    /// ILoadingScreen is an interface that all
    /// loading screens must implement. This adds 
    /// an api for objects to interact with the 
    /// the different states of a loading screen.
    /// </summary>
    public interface ILoadingScreen
    {
        Action OnTransitionedIn { get; set; }
        Action OnTransitionedOut { get; set; }

        void TransitionIn();
        void TransitionOut();

        void TransitionIn(Action callback);
        void TransitionOut(Action callback);
    }

    public enum LoadingScreenState
    {
        OFF,
        TRANSITION_IN,
        ON,
        TRANSITION_OUT
    }
}
