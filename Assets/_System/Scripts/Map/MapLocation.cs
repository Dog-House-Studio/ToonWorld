using UnityEngine;
using DogHouse.ToonWorld.Services;

namespace DogHouse.ToonWorld.Map
{
    /// <summary>
    /// MapLocation is a scriptable object that
    /// represents a particular node in a 
    /// overworld map. This is used to identify
    /// the different types of locations that the
    /// player could go to.
    /// </summary>
    [CreateAssetMenu(fileName = "MyNewAssetToken",
        menuName = "Dog House/ToonWorld/Map Location")]
    public class MapLocation : ScriptableObject
    {
        #region Public Variables
        public Sprite LocationSprite => m_locationSprite;
        public string LocationName => m_locationName;
        public GameSceneDefinition SceneDefinition => m_sceneDefinition;
        #endregion

        #region Private Variables
        [SerializeField]
        private Sprite m_locationSprite;

        [SerializeField]
        private string m_locationName;

        [SerializeField]
        private GameSceneDefinition m_sceneDefinition;
        #endregion
    }
}
