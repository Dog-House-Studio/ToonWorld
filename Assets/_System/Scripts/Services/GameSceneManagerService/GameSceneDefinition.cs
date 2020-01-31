using UnityEngine;
using DogHouse.ScaffoldCandidates.AssetManagement;
using UnityEngine.SceneManagement;

namespace DogHouse.ToonWorld.Services
{
    /// <summary>
    /// GameSceneDefinition is a scriptable object
    /// that serializes a scene and how it should be 
    /// loaded.
    /// </summary>
    [CreateAssetMenu(fileName = "MyNewGameSceneDefinition",
        menuName = "Dog House/Asset Management/Game Scene Definition")]
    public class GameSceneDefinition : ScriptableObject
    {
        #region Public Variables
        public AssetToken Token => m_sceneToken;
        public LoadSceneMode Mode => m_mode;
        public TransitionType Type => m_transitionType;
        #endregion

        #region Private Variables
        [SerializeField]
        private AssetToken m_sceneToken;

        [SerializeField]
        private LoadSceneMode m_mode;

        [SerializeField]
        private TransitionType m_transitionType;
        #endregion
    }

    public enum TransitionType
    {
        Fade,
        LoadingScreen,
        None
    }
}
