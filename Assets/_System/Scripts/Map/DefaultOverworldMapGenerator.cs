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
        [Header("Objects")]
        [SerializeField]
        private GameObject m_startLocation;

        [SerializeField]
        private GameObject m_endLocation;

        [SerializeField]
        private GameObject m_mapLocationPrefab;

        [SerializeField]
        private MapLocationInfo[] m_locations;
        #endregion

        #region Main Methods
        private void Start()
        {
            Generate();
        }

        public void Display(bool value)
        {
            
        }

        public void Generate()
        {
            GameObject start = Instantiate(m_mapLocationPrefab);
            start.transform.position = m_startLocation.transform.position;

            GameObject end = Instantiate(m_mapLocationPrefab);
            end.transform.position = m_endLocation.transform.position;
        }

        public void SetSeed(int seedValue)
        {
            
        }
        #endregion
    }

    [System.Serializable]
    public struct MapLocationInfo
    {
        public MapLocation m_mapLocation;
        public MapLocationType m_type;
    }

    public enum MapLocationType
    {
        START,
        MIDDLE,
        END
    }
}
