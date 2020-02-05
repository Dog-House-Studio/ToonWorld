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

            Node endNode = CreateNode(m_locations[0].m_mapLocation);
            Vector3 pos = new Vector3(m_endLocation.transform.position.x,height + 3f,-0.25f);
            endNode.SetPosition(pos);
            List<Node> endRow = new List<Node>();
            
            endRow.Add(endNode);
            rows.Add(endRow);

            GenerateConnections(rows);
            for(int i = 0; i < rows.Count; i++)
            {
                for (int j = 0; j < rows[i].Count;j++)
                {
                    rows[i][j].CreateLineRenders();
                }
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
            //if(numberOfIcons == lastRowCount)
            //{
            //    numberOfIcons = UnityEngine.Random.Range(m_minNumberOfIconsPerRow, m_maximumNumberOfIconsPerRow);
            //}

            List<Node> nodes = new List<Node>();
            float segmentLengths = m_rowWidth / numberOfIcons;

            for(int i = 0; i < numberOfIcons; i++)
            {
                nodes.Add(CreateNode(m_locations[0].m_mapLocation));
                Vector3 pos = Vector3.zero;
                pos.z = -0.25f;
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

            newNode.SetData(location);
            m_nodeWeb.AddNode(newNode);
            return newNode;
        }

        private void GenerateConnections(List<List<Node>> rows)
        {
            //Looping the rows backwards
            for(int i = rows.Count - 2; i >= 0; i--)
            {
                int topCount = rows[i + 1].Count;
                int bottomCount = rows[i].Count;

                //Same number on top as bottom
                if(topCount == bottomCount)
                {
                    ConnectDirectly(rows[i], rows[i + 1]);
                    continue;
                }

                if(topCount > bottomCount)
                {
                    ConnectTopHeavy(rows[i], rows[i + 1]);
                    continue;
                }

                ConnectBottomHeavy(rows[i], rows[i + 1]);
            }
        }

        private void ConnectBottomHeavy(List<Node> bottom, List<Node> top)
        {
            List<Node> unusedBottom = new List<Node>();
            for(int i = 0; i < bottom.Count; i++)
            {
                unusedBottom.Add(bottom[i]);
            }

            for(int i = 0; i < top.Count; i++)
            {
                float closestDistance = float.MaxValue;
                Node closestNode = null;

                for(int j = 0; j < bottom.Count; j++)
                {
                    float distance = Mathf.Abs(top[i].Position.x - bottom[j].Position.x);
                    if(distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestNode = bottom[j];
                    }
                }

                closestNode.SetOutput(top[i]);
                unusedBottom.Remove(closestNode);
            }

            for(int i = 0; i < unusedBottom.Count; i++)
            {
                float closestDistance = float.MaxValue;
                Node closestNode = null;

                for(int j = 0; j < top.Count;j++)
                {
                    float distance = Mathf.Abs(top[j].Position.x - unusedBottom[i].Position.x);
                    if(distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestNode = top[j];
                    }
                }

                unusedBottom[i].SetOutput(closestNode);
            }
        }

        private void ConnectTopHeavy(List<Node> bottom, List<Node> top)
        {
            List<Node> unusedTop = new List<Node>();
            for (int i = 0; i < top.Count; i++)
            {
                unusedTop.Add(top[i]);
            }

            for (int i = 0; i < bottom.Count; i++)
            {
                float closestDistance = float.MaxValue;
                Node closestNode = null;

                for (int j = 0; j < top.Count; j++)
                {
                    float distance = Mathf.Abs(bottom[i].Position.x - top[j].Position.x);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestNode = top[j];
                    }
                }

                closestNode.SetOutput(bottom[i]);
                unusedTop.Remove(closestNode);
            }

            for (int i = 0; i < unusedTop.Count; i++)
            {
                float closestDistance = float.MaxValue;
                Node closestNode = null;

                for (int j = 0; j < bottom.Count; j++)
                {
                    float distance = Mathf.Abs(bottom[j].Position.x - unusedTop[i].Position.x);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestNode = bottom[j];
                    }
                }

                unusedTop[i].SetOutput(closestNode);
            }
        }

        private void ConnectDirectly(List<Node> bottom, List<Node> top)
        {
            for(int i = 0; i < bottom.Count; i++)
            {
                bottom[i].SetOutput(top[i]);
            }
        }
        #endregion
    }
}
