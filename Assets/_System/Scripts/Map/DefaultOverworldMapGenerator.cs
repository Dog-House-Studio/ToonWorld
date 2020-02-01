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

        [Header("Generation Settings")]
        [SerializeField]
        [Range(0.0001f, 1f)]
        float m_minPlacementRange;

        [SerializeField]
        [Range(0.0001f, 1f)]
        float m_maximumPlacementRange;

        [SerializeField]
        int m_MinimumNumberOfBranches;

        [SerializeField]
        int m_maximumNumberOfBranches;

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
            Node StartNode = CreateNode(m_locations[0].m_mapLocation);
            StartNode.SetPosition(m_startLocation.transform.position);

            Node EndNode = CreateNode(m_locations[1].m_mapLocation);
            EndNode.SetPosition(m_endLocation.transform.position);

            StartNode.SetOutput(EndNode);

            int numberOfBranches = Random.Range(m_MinimumNumberOfBranches, m_maximumNumberOfBranches + 1);
            for(int i = 0; i < numberOfBranches; i++)
            {

            }
        }

        public void SetSeed(int seedValue)
        {
            CreateBranch();
        }
        #endregion

        #region Utility Methods
        private Node CreateNode(MapLocation location)
        {
            Node newNode = new Node();

            GameObject nodeObject = Instantiate(m_mapLocationPrefab);
            nodeObject.transform.SetParent(m_nodeParent.transform);
            newNode.m_nodeRootGameObject = nodeObject;

            newNode.m_visualController = nodeObject
                   .GetComponent<MapLocationVisualController>();

            newNode.m_visualController.SetIcon(location.LocationSprite);
            return newNode;
        }

        private void CreateBranch()
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
