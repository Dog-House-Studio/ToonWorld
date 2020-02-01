﻿using System.Collections.Generic;
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

        public GameObject m_nodeRootGameObject;
        public MapLocationVisualController m_visualController;
        public List<Node> m_inputs = new List<Node>();
        public List<Node> m_outputs = new List<Node>();

        public void SetPosition(Vector3 position)
        {
            m_nodeRootGameObject.transform.position = position;
        }

        public void SetOutput(Node node)
        {
            m_outputs.Add(node);
            node.m_inputs.Add(this);

            m_visualController.SetOutput(node.m_nodeRootGameObject);
        }

        public float Distance(Node node)
        {
            return (node.Position -Position).magnitude;
        }
    }
}
