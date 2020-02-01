using UnityEngine;

namespace DogHouse.ToonWorld.Map
{
    /// <summary>
    /// DefaultOverworldMapGenerator is a script that
    /// implements the IOverworldMapGenerator. This 
    /// implementation creates the map similar to how
    /// the maps are generated in slay the spire.
    /// </summary>
    public class DefaultOverworldMapGenerator : MonoBehaviour, 
        IOverworldMapGenerator
    {
        #region Private Variables
        [SerializeField]
        private MapLocation[] m_locations;
        #endregion

        #region Main Methods
        public void Display(bool value)
        {
            
        }

        public void Generate()
        {
            
        }

        public void SetSeed(int seedValue)
        {
            
        }
        #endregion
    }
}
