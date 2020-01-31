using DogScaffold;
using UnityEngine.SceneManagement;
using DogHouse.CoreServices;

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
        #region Private Variables
        private ServiceReference<ILoadingScreenService> m_loadingScreenService 
                = new ServiceReference<ILoadingScreenService>();

        private ServiceReference<ICameraTransition> m_cameraTransition 
            = new ServiceReference<ICameraTransition>();
        #endregion

        #region Main Methods
        public void LoadScene(GameSceneDefinition sceneDefinition)
        {
            SceneManager.LoadScene(sceneDefinition.Token.AssetName);
        }
        #endregion
    }
}
