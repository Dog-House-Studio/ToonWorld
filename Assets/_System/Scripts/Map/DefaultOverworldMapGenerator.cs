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
        private GameObject m_nodeParent;

        [SerializeField]
        private GameObject m_mapLocationPrefab;

        [SerializeField]
        private MapLocationInfo[] m_locations;

        private NodeWeb m_nodeWeb = new NodeWeb();
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
            start.transform.SetParent(m_nodeParent.transform);
            MapLocationVisualController startController = start.GetComponent<MapLocationVisualController>();
            startController.SetIcon(m_locations[0].m_mapLocation.LocationSprite);
            
            //Node startNode = new Node();
            //startNode.m_nodeRootGameObject = start;

            GameObject end = Instantiate(m_mapLocationPrefab);
            end.transform.position = m_endLocation.transform.position;
            end.transform.SetParent(m_nodeParent.transform);
            MapLocationVisualController endController = end.GetComponent<MapLocationVisualController>();
            endController.SetIcon(m_locations[1].m_mapLocation.LocationSprite);

            startController.SetOutput(end);
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
