using System.Collections.Generic;
using UnityEngine;

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
        #endregion

        #region Private Variables
        private List<Node> m_nodes = new List<Node>();
        #endregion

        #region Main Methods
        public void AddNode(Node node)
        {
            m_nodes.Add(node);
        }
        #endregion
    }

    /// <summary>
    /// A node is a single part of a 
    /// node web.
    /// </summary>
    public class Node
    {
        public Vector3 Position => m_nodeRootGameObject.transform.position;

        public static float MinXPosition;
        public static float MaxXPosition;

        public GameObject m_nodeRootGameObject;
        public MapLocationVisualController m_visualController;
        public List<Node> m_inputs = new List<Node>();
        public List<Node> m_outputs = new List<Node>();

        public void SetPosition(Vector3 position)
        {
            position.x = Mathf.Max(MinXPosition, position.x);
            position.x = Mathf.Min(MaxXPosition, position.x);

            m_nodeRootGameObject.transform.position = position;
        }

        public void SetOutput(Node node)
        {
            m_outputs.Add(node);
            node.m_inputs.Add(this);
        }

        public void RemoveOutput(Node node)
        {
            if (!m_outputs.Contains(node)) return;

            m_outputs.Remove(node);
        }

        public void CreateLineRenders()
        {
            for(int i = 0; i < m_outputs.Count; i++)
            {
                m_visualController.SetOutput(m_outputs[i].m_nodeRootGameObject);
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
