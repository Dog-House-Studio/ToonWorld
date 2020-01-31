using DogScaffold;
using UnityEngine.SceneManagement;
using DogHouse.CoreServices;
using UnityEngine;
using ILoadingScreenService = DogHouse.ToonWorld.Services.ILoadingScreenService;

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
        [SerializeField]
        [Range(0f, 5f)]
        private float m_fadeTime;

        private ServiceReference<ILogService> m_logService 
                = new ServiceReference<ILogService>();

        private ServiceReference<ILoadingScreenService> m_loadingScreenService 
                = new ServiceReference<ILoadingScreenService>();

        private ServiceReference<ICameraTransition> m_cameraTransition 
            = new ServiceReference<ICameraTransition>();

        private SceneManagerState m_state = SceneManagerState.IDLE;
        #endregion

        #region Main Methods
        public void LoadScene(GameSceneDefinition sceneDefinition)
        {
            if(m_state == SceneManagerState.LOADING)
            {
                m_logService?.Reference?.LogError("Already loading scene!");
                return;
            }
            m_state = SceneManagerState.LOADING;

            if(sceneDefinition.Type == TransitionType.None)
            {
                Load(sceneDefinition);
                return;
            }
            
            if(sceneDefinition.Type == TransitionType.Fade)
            {
                m_cameraTransition?.Reference?.FadeIn(m_fadeTime, 
                    () => { FadeInLoad(sceneDefinition); });
                return;
            }

            m_loadingScreenService?.Reference?.TransitionIn(
                () => { LoadingScreenLoadedIn(sceneDefinition); });
        }
        #endregion

        #region Utility Methods
        private void LoadingScreenLoadedIn(GameSceneDefinition definition)
        {
            SceneManager.LoadScene(definition.Token.AssetName, definition.Mode);
            m_loadingScreenService?.Reference?.TransitionOut(FinishLoading);
        }

        private void FadeInLoad(GameSceneDefinition definition)
        {
            SceneManager.LoadScene(definition.Token.AssetName, definition.Mode);
            m_cameraTransition.Reference?.FadeOut(m_fadeTime, FinishLoading);
        }

        private void Load(GameSceneDefinition definition)
        {
            SceneManager.LoadScene(definition.Token.AssetName,definition.Mode);
            FinishLoading();
        }

        private void FinishLoading()
        {
            m_state = SceneManagerState.IDLE;
        }
        #endregion
    }
}
