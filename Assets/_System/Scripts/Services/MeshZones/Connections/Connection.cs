using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

namespace DogHouse.ToonWorld.Services
{
    /// <summary>
    /// A connection is a class which represents
    /// some form of a connection between two 
    /// points on a ground tile.
    /// </summary>
    public class Connection
    {
        public Vector3 root;
        public List<Connection> connections = new List<Connection>();
        public List<Vector3> sharedTiles = new List<Vector3>();

        public void RemoveInvalidConnections()
        {
            for (int i = connections.Count - 1; i >= 0; i--)
            {
                if (!isValidConnection(this, connections[i]))
                {
                    connections.RemoveAt(i);
                }
            }
        }

        public bool HasConnection(Connection connection)
        {
            return connections.Contains(connection);
        }

        public static bool isValidConnection(Connection a, Connection b)
        {
            //This shouldn't be hardcoded
            if (!Approximately(Vector3.Distance(a.root, b.root), 1f)) return false;

            int sharedCount = 0;
            for (int i = 0; i < a.sharedTiles.Count; i++)
            {
                for (int j = 0; j < b.sharedTiles.Count; j++)
                {
                    if (a.sharedTiles[i].Equals(b.sharedTiles[j]))
                    {
                        sharedCount++;
                        if (sharedCount > 1) return false;
                    }
                }
            }

            return (sharedCount == 1);
        }
    }
}
