using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
        [Range(0.0001f, 10f)]
        float m_endZonePlacementRange;

        [SerializeField]
        float m_minXPosition;

        [SerializeField]
        float m_maxXPosition;

        [SerializeField]
        [Range(0.0001f, 10f)]
        float m_minPlacementRange;

        [SerializeField]
        [Range(0.0001f, 10f)]
        float m_maximumPlacementRange;

        [SerializeField]
        [Range(0.0001f, 10f)]
        float m_maxSwayRange;

        [SerializeField]
        int m_MinimumNumberOfBranches;

        [SerializeField]
        int m_maximumNumberOfBranches;

        [SerializeField]
        [Range(0.0001f, 10f)]
        float m_paddingSpace;

        [SerializeField]
        int m_paddingItterations;

        [SerializeField]
        [Range(0.0001f, 10f)]
        private float m_connectionDistance;

        [SerializeField]
        [Range(0f, 1f)]
        private float m_connectionDotValue;

        private NodeWeb m_nodeWeb = new NodeWeb();
        #endregion

        #region Main Methods
        public void Display(bool value)
        {
            
        }

        public NodeWeb Generate()
        {
            List<Node> ignoreList = new List<Node>();
            Node.MinXPosition = m_minXPosition;
            Node.MaxXPosition = m_maxXPosition;

            //Start Node
            Node StartNode = CreateNode(m_locations[0].m_mapLocation);
            StartNode.SetPosition(m_startLocation.transform.position);
            m_nodeWeb.Start = StartNode;

            //End Node
            Node EndNode = CreateNode(m_locations[1].m_mapLocation);
            EndNode.SetPosition(m_endLocation.transform.position);
            m_nodeWeb.End = EndNode;

            //End zones
            int numberOfBranches = UnityEngine.Random.Range(m_MinimumNumberOfBranches, m_maximumNumberOfBranches + 1);
            List<Node> m_endPositionNodes = new List<Node>();
            for (int i = 0; i < numberOfBranches; i++)
            {
                Node newNode = CreateNode(m_locations[0].m_mapLocation);
                Vector3 offset = Vector3.zero;
                offset.y = Mathf.Lerp(-m_minPlacementRange, -m_maximumPlacementRange, 0.5f);

                offset.x = (EndNode.Position.x - (m_endZonePlacementRange / 2)) + (m_endZonePlacementRange / (numberOfBranches + 1)) * (i + 1);
                newNode.SetPosition(EndNode.Position + offset);
                newNode.SetOutput(EndNode);
                m_endPositionNodes.Add(newNode);
                ignoreList.Add(newNode);
            }

            //Branches
            for (int i = 0; i < numberOfBranches; i++)
            {
                Node newNode = CreateNode(m_locations[0].m_mapLocation);
                Vector3 offset = Vector3.zero;
                offset.y = Mathf.Lerp(m_minPlacementRange, m_maximumPlacementRange, 0.5f);

                offset.x = (StartNode.Position.x - (m_endZonePlacementRange / 2)) + (m_endZonePlacementRange / (numberOfBranches + 1)) * (i + 1);
                newNode.SetPosition(StartNode.Position + offset);
                StartNode.SetOutput(newNode);
                ignoreList.Add(newNode);
                CreateBranch(newNode, m_endPositionNodes[i]);
            }

            
            ignoreList.Add(EndNode);
            ignoreList.Add(StartNode);
            ResolvePaddings(ignoreList);
            ModifyConnections(ignoreList);
            for(int i = 0; i < m_nodeWeb.Nodes.Count; i++)
            {
                m_nodeWeb.Nodes[i].CreateLineRenders();
            }

            return m_nodeWeb;
        }

        public void SetSeed(int seedValue)
        {

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

            newNode.SetData(location);
            m_nodeWeb.AddNode(newNode);
            return newNode;
        }

        private void CreateBranch(Node RootBranch, Node BranchTip)
        {
            float distance = RootBranch.Distance(BranchTip);
            float orginalDistance = distance;
            Node LastNode = RootBranch;
            Vector3 tempPosition = Vector3.zero;

            do
            {
                Node newNode = CreateNode(m_locations[0].m_mapLocation);
                

                Vector3 Offset = Vector3.zero;
                Offset.y += UnityEngine.Random.Range(m_minPlacementRange, m_maximumPlacementRange);
                Offset.x += UnityEngine.Random.Range(-m_maxSwayRange, m_maxSwayRange);
                newNode.SetPosition(LastNode.Position + Offset);
                tempPosition = Vector3.Lerp(newNode.Position, BranchTip.Position, newNode.Distance(RootBranch) / orginalDistance);
                tempPosition.y = newNode.Position.y;
                newNode.SetPosition(tempPosition);
                LastNode.SetOutput(newNode);
                LastNode = newNode;
                distance = LastNode.Distance(BranchTip);

            } while (distance > m_maximumPlacementRange);

            LastNode.SetOutput(BranchTip);
        }

        private void ResolvePaddings(List<Node> ignoreList)
        {
            List<Node> CloseList = new List<Node>();

            for (int itteration = 0; itteration < m_paddingItterations; itteration++)
            {
                for (int currentNode = 0; currentNode < m_nodeWeb.Nodes.Count; currentNode++)
                {
                    Node node = m_nodeWeb.Nodes[currentNode];
                    if (ignoreList.Contains(node)) continue;

                    CloseList.Clear();
                    //Calculate close list
                    for (int i = 0; i < m_nodeWeb.Nodes.Count; i++)
                    {
                        if (i == currentNode) continue;
                        if (node.Distance(m_nodeWeb.Nodes[i]) < m_paddingSpace)
                        {
                            CloseList.Add(m_nodeWeb.Nodes[i]);
                        }
                    }

                    Vector3 averagePosition = Node.AveragePosition(CloseList.ToArray());
                    Vector3 toAverage = averagePosition - node.Position;
                    toAverage.Normalize();
                    Vector3 awayVector = toAverage;
                    awayVector.x = -awayVector.x;
                    awayVector.y = -awayVector.y;
                    awayVector.z = 0f;

                    node.SetPosition(node.Position + (awayVector * m_paddingSpace * 0.5f));
                }
            }
        }

        private void ModifyConnections(List<Node> ignoreList)
        {
            List<Node> orderList = new List<Node>();
            //Get all the nodes into the list except the ones were ignoring.
            for(int i = 0; i < m_nodeWeb.Nodes.Count; i++)
            {
                if (ignoreList.Contains(m_nodeWeb.Nodes[i])) continue;
                orderList.Add(m_nodeWeb.Nodes[i]);
            }

            orderList = orderList.OrderBy(x => x.Position.y).ToList();

            for(int i = 0; i < orderList.Count; i++)
            {
                List<Node> closeNodes = new List<Node>();

                //Get all nodes that are close enough for a connection but not connected yet
                for (int j = i + 1; j < orderList.Count; j++)
                {
                    if (orderList[i].m_outputs.Contains(orderList[j])) continue;

                    if(orderList[i].Distance(orderList[j]) < m_connectionDistance)
                    {
                        closeNodes.Add(orderList[j]);
                    }
                }

                for(int j = 0; j < closeNodes.Count; j++)
                {
                    Vector3 toPrimary = orderList[i].m_outputs[0].Position - orderList[i].Position;
                    toPrimary.Normalize();

                    Vector3 toCloseNode = closeNodes[j].Position - orderList[i].Position;
                    toCloseNode.Normalize();

                    if(Vector3.Dot(toPrimary, toCloseNode) > m_connectionDotValue)
                    {
                        closeNodes[j].SetOutput(orderList[i].m_outputs[0]);
                        orderList[i].RemoveOutput(orderList[i].m_outputs[0]);
                        
                    }

                    orderList[i].SetOutput(closeNodes[j]);
                }
            }
        }
        #endregion
    }

    [System.Serializable]
    public struct MapLocationInfo
    {
        public MapLocation m_mapLocation;
        public MapLocationType m_type;

        public bool m_useStaticNumber;
        public int m_munNumberOfInstances;
        public int m_maxNumberOfInstances;

        [Range(0f,1f)]
        public float m_oddsWeight;
    }

    public enum MapLocationType
    {
        START,
        MIDDLE,
        END
    }
}
