using System.Collections.Generic;
using UnityEngine;
using DogScaffold;
using DogHouse.ToonWorld.Services;

namespace DogHouse.ToonWorld.Map
{
    /// <summary>
    /// NodeWeb is a collection of nodes
    /// and maps the connections between
    /// the different nodes.
    /// </summary>
    public class NodeWeb
    {
        #region Public Variables
        public List<Node> Nodes => m_nodes;
        public Node Start;
        public Node End;
        #endregion

        #region Private Variables
        private List<Node> m_nodes = new List<Node>();
        #endregion

        #region Main Methods
        public void AddNode(Node node)
        {
            m_nodes.Add(node);
        }

        public int ContainsCount(MapLocation locationType)
        {
            int count = 0;
            foreach(Node node in m_nodes)
            {
                if(node.IconType == locationType)
                {
                    count++;
                }
            }
            return count;
        }
        #endregion
    }

    /// <summary>
    /// A node is a single part of a 
    /// node web.
    /// </summary>
    public class Node
    {
        public List<Node> Outputs => m_outputs;


        public MapLocation IconType => m_data;
        public Vector3 Position => m_nodeRootGameObject.transform.position;

        public GameObject m_nodeRootGameObject;
        public MapLocationVisualController m_visualController;

        private List<Node> m_outputs = new List<Node>();

        private ServiceReference<IMapService> m_mapService 
            = new ServiceReference<IMapService>();

        private MapLocation m_data;

        public void SetData(MapLocation location)
        {
            m_visualController.SetIcon(location.LocationSprite);
            m_data = location;
        }

        public void SetIconSelectedColor(bool value)
        {
            m_visualController?.SetIconSelectedColor(value);
        }

        public void SetAsActiveOption(bool value)
        {
            m_visualController.SetFull(value);
            m_visualController?.SetIconActive(value);

            m_visualController.OnClicked -= OnClicked;
            m_visualController.OnClicked += OnClicked;
        }

        private void OnClicked()
        {
            if (!m_mapService.CheckServiceRegistered()) return;
            m_mapService.Reference.ReportIconSelected(this);
        }

        public void SetPosition(Vector3 position)
        {
            m_nodeRootGameObject.transform.position = position;
        }

        public void SetOutput(Node node)
        {
            m_outputs.Add(node);
        }

        public void CreateLineRenders()
        {
            for(int i = 0; i < Outputs.Count; i++)
            {
                m_visualController.SetOutput(Outputs[i].m_nodeRootGameObject);
            }
        }

        public float Distance(Node node)
        {
            return (node.Position -Position).magnitude;
        }

        public static Vector3 AveragePosition(Node[] nodes)
        {
            Vector3 position = Vector3.zero;
            for(int i = 0; i < nodes.Length; i++)
            {
                position += nodes[i].Position;
            }

            position.x /= nodes.Length;
            position.y /= nodes.Length;
            return position;
        }
    }
}
