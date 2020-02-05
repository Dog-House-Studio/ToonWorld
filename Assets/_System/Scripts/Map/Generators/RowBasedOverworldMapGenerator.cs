using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace DogHouse.ToonWorld.Map
{
    /// <summary>
    /// RowBasedOverworldMapGenerator is a row based 
    /// approach to generating maps. 
    /// </summary>
    public class RowBasedOverworldMapGenerator : MonoBehaviour, IOverworldMapGenerator
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

        [Header("Rows")]
        [SerializeField]
        private int m_minNumberOfIconsPerRow;

        [SerializeField]
        private int m_maximumNumberOfIconsPerRow;

        [SerializeField]
        private int m_numberOfRows;

        [SerializeField]
        [Range(0.0001f, 10f)]
        private float m_rowHeight;

        [SerializeField]
        [Range(0.0001f, 10f)]
        private float m_rowWidth;

        private NodeWeb m_nodeWeb = new NodeWeb();
        #endregion

        #region Main Methods
        public void Display(bool value)
        {

        }

        public NodeWeb Generate()
        {
            List<Node> ignoreList = new List<Node>();
            //Node.MinXPosition = m_minXPosition;
            //Node.MaxXPosition = m_maxXPosition;

            List<List<Node>> rows = new List<List<Node>>();
            float height = m_startLocation.transform.position.y;
            int lastRowCount = 0;

            for (int i = 0; i < m_numberOfRows; i++)
            {
                rows.Add(CreateRow(ref height, lastRowCount));
                lastRowCount = rows[i].Count;
            }


            Vector3 endPosition = m_endLocation.transform.position;
            endPosition.y = height;
            m_endLocation.transform.position = endPosition;

            return m_nodeWeb;
        }

        public void SetSeed(int seedValue)
        {

        }
        #endregion

        #region Utility Methods
        private List<Node> CreateRow(ref float height, int lastRowCount)
        {
            int numberOfIcons = UnityEngine.Random.Range(m_minNumberOfIconsPerRow, m_maximumNumberOfIconsPerRow);
            if(numberOfIcons == lastRowCount)
            {
                numberOfIcons = UnityEngine.Random.Range(m_minNumberOfIconsPerRow, m_maximumNumberOfIconsPerRow);
            }

            List<Node> nodes = new List<Node>();
            float segmentLengths = m_rowWidth / numberOfIcons;

            for(int i = 0; i < numberOfIcons; i++)
            {
                nodes.Add(CreateNode(m_locations[0].m_mapLocation));
                Vector3 pos = Vector3.zero;
                pos.z = 0.25f;
                pos.y = height + (m_rowHeight * 0.5f);
                pos.x = segmentLengths * i + (segmentLengths * 0.5f) - (m_rowWidth * 0.5f);

                Vector2 randomOffset = UnityEngine.Random.insideUnitCircle* segmentLengths * 0.25f;
                pos.x += randomOffset.x;
                pos.y += randomOffset.y;

                nodes[i].SetPosition(pos);
            }

            height += m_rowHeight;
            return nodes;
        }

        private Node CreateNode(MapLocation location)
        {
            Node newNode = new Node();

            GameObject nodeObject = Instantiate(m_mapLocationPrefab);
            nodeObject.transform.SetParent(m_nodeParent.transform);
            newNode.m_nodeRootGameObject = nodeObject;

            newNode.m_visualController = nodeObject
                   .GetComponent<MapLocationVisualController>();

            newNode.SetData(FetchMapLocationType());
            m_nodeWeb.AddNode(newNode);
            return newNode;
        }

        private MapLocation FetchMapLocationType()
        {
            List<MapLocationInfo> availableMapLocationTypes = new List<MapLocationInfo>();
            availableMapLocationTypes = m_locations.Where(x => x.m_type == MapLocationType.MIDDLE).ToList();

            for (int i = availableMapLocationTypes.Count - 1; i >= 0; i--)
            {
                if (!availableMapLocationTypes[i].m_useStaticNumber) continue;
                if (m_nodeWeb.ContainsCount(availableMapLocationTypes[i].m_mapLocation) >=
                    availableMapLocationTypes[i].m_maxNumberOfInstances)
                {
                    availableMapLocationTypes.RemoveAt(i);
                }
            }
            int index = UnityEngine.Random.Range(0, availableMapLocationTypes.Count);
            return availableMapLocationTypes[index].m_mapLocation;
        }
        #endregion
    }
}
