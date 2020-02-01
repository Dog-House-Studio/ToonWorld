using UnityEngine;
using DogScaffold;

namespace DogHouse.ToonWorld.Services
{
    /// <summary>
    /// GameSceneManagerBackdoor is a convenient back door
    /// to the service for other objects to use like the
    /// actual service.
    /// </summary>
    public class GameSceneManagerBackdoor : MonoBehaviour, IGameSceneManagerService
    {
        #region Private Variables
        private ServiceReference<IGameSceneManagerService> m_sceneManager 
            = new ServiceReference<IGameSceneManagerService>();
        #endregion

        #region Main Methods
        public void LoadScene(GameSceneDefinition sceneDefinition)
        {
            if (!m_sceneManager.CheckServiceRegistered()) return;
            m_sceneManager?.Reference?.LoadScene(sceneDefinition);
        }

        public void RegisterService() {}
        #endregion
    }
}
