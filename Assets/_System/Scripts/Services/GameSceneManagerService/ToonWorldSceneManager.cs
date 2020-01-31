using DogScaffold;
using UnityEngine.SceneManagement;

namespace DogHouse.ToonWorld.Services
{
    /// <summary>
    /// This script implements the IGameSceneManagerService
    /// interface. This script is responsible for loading
    /// the scenes of the game.
    /// </summary>
    public class ToonWorldSceneManager : BaseService<IGameSceneManagerService>, 
        IGameSceneManagerService
    {
        #region Main Methods
        public void LoadScene(GameSceneDefinition sceneDefinition)
        {
            SceneManager.LoadScene(sceneDefinition.Token.AssetName);
        }
        #endregion
    }
}
