using DogScaffold;
using System;

namespace DogHouse.ToonWorld.Services
{
    /// <summary>
    /// IGameSceneManagerService is a modular scene
    /// manager for ToonWorld.
    /// </summary>
    public interface IGameSceneManagerService : IService
    {
        void LoadScene(GameSceneDefinition sceneDefinition, Action callback = null);
    }

    public enum SceneManagerState
    {
        IDLE,
        LOADING
    }
}
