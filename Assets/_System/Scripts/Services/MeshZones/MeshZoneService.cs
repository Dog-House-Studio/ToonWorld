using System.Collections.Generic;
using UnityEngine;
using DogScaffold;
using static UnityEngine.Mathf;
using System.Linq;
using System;

namespace DogHouse.ToonWorld.Services
{
    /// <summary>
    /// MeshZoneService is an implementation of
    /// the IMeshZoneService. This service is 
    /// responsible for generating a mesh at runtime
    /// which shows a particular zone.
    /// </summary>
    public class MeshZoneService : BaseService<IMeshZoneService>, IMeshZoneService
    {
        #region Private Variables
        [SerializeField]
        public float m_tileSize;

        [SerializeField]
        private Vector3 m_tileOffset;

        [SerializeField]
        private Material m_zoneMaterial;

        [SerializeField]
        private GameObject m_cubePrefab;

        private List<ConnectionRing> m_rings = new List<ConnectionRing>();
        #endregion

        #region Main Methods
        public GameObject GenerateZone(Vector3[] tileLocations)
        {
            List<Vector3> insideVerts = new List<Vector3>();
            List<int> insideIndices = new List<int>();
            GenerateInsideMeshData(tileLocations, ref insideVerts, ref insideIndices);
            Mesh insideMesh = GenerateMesh(insideVerts, insideIndices);

            List<Vector3> edgeVerts = new List<Vector3>();
            List<int> edgeIndices = new List<int>();
            GenerateEdgeMeshData(tileLocations, ref edgeVerts, ref edgeIndices, insideVerts);
            Mesh edgeMesh = GenerateMesh(edgeVerts, edgeIndices);

            return SetupZoneObject(insideMesh, m_zoneMaterial);
        }

        private void GenerateEdgeMeshData(Vector3[] tileLocations, ref List<Vector3> edgeVerts, ref List<int> edgeIndices, List<Vector3> insideVerts)
        {
            List<Vector3> edgeTiles = ExtractEdgeTiles(tileLocations);
            List<Vector3> perimeterTiles = DeterminePermimeterTiles(edgeTiles, tileLocations.ToList());
            List<Vector3> edgeVertices = CalculateEdgeVertices(edgeTiles, perimeterTiles);
            List<Connection> connections = GenerateConnections(edgeVertices, tileLocations.ToList());

            for (int i = 0; i < connections.Count; i++)
            {
                GameObject point = Instantiate(m_cubePrefab);
                point.transform.position = connections[i].root;

                MeshRenderer renderer = point.GetComponent<MeshRenderer>();
                Material material = renderer.material;

                if (connections[i].connections.Count == 2)
                {
                    material.SetColor("_BaseColor", Color.green);
                }

                if (connections[i].connections.Count == 3)
                {
                    material.SetColor("_BaseColor", new Color(1f, 0.61f, 0f, 1f));
                }

                if (connections[i].connections.Count == 4)
                {
                    material.SetColor("_BaseColor", Color.red);
                }

                renderer.material = material;

                ConnectionPointVisualization visualizer = point.GetComponent<ConnectionPointVisualization>();
                visualizer.connection = connections[i];
            }

            m_rings.Clear();
            m_rings = GenerateConnectionRings(connections);
        }

        private void GenerateInsideMeshData(Vector3[] tileLocations, 
            ref List<Vector3> verts, ref List<int> indices)
        {
            //Tiles in between
            for (int i = 0; i < tileLocations.Length; i++)
            {
                CalculateTileVerts(tileLocations[i], ref verts, ref indices);
            }
        }

        private void CalculateTileVerts(Vector3 location, ref List<Vector3> verts, ref List<int> indices)
        {
            int lastIndex = verts.Count - 1;
            float amount = 0.5f * m_tileSize;
            location += m_tileOffset;

            Vector3 vert1 = location;
            vert1.z += amount;
            vert1.x += amount;

            Vector3 vert2 = location;
            vert2.z -= amount;
            vert2.x += amount;

            Vector3 vert3 = location;
            vert3.z -= amount;
            vert3.x -= amount;

            Vector3 vert4 = location;
            vert4.z += amount;
            vert4.x -= amount;

            verts.Add(vert1);
            verts.Add(vert2);
            verts.Add(vert3);
            verts.Add(vert4);

            indices.Add(lastIndex + 1);
            indices.Add(lastIndex + 2);
            indices.Add(lastIndex + 3);

            indices.Add(lastIndex + 1);
            indices.Add(lastIndex + 3);
            indices.Add(lastIndex + 4);
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            for(int i = 0; i < m_rings.Count; i++)
            {
                if (i == 1) Gizmos.color = Color.green;
                if (i == 2) Gizmos.color = Color.blue;
                if (i == 3) Gizmos.color = Color.gray;
                if (i == 4) Gizmos.color = Color.cyan;

                for(int j = 0; j < m_rings[i].Ring.Count; j++)
                {
                    Gizmos.DrawLine(m_rings[i].Ring[j].root + Vector3.up * (0.1f * i), m_rings[i].Ring[(j + 1) % m_rings[i].Ring.Count].root + Vector3.up * (0.1f * i));
                    UnityEditor.Handles.Label(m_rings[i].Ring[j].root + Vector3.up * (0.1f * i), j.ToString());
                }
            }
        }
        #endregion

        #region Utility Methods
        private GameObject SetupZoneObject(Mesh mesh, Material material)
        {
            GameObject zoneObject = new GameObject();
            zoneObject.transform.SetParent(transform);
            zoneObject.transform.localPosition = Vector3.zero;
            zoneObject.transform.localRotation = Quaternion.identity;
            MeshRenderer renderer = zoneObject.AddComponent<MeshRenderer>();
            MeshFilter filter = zoneObject.AddComponent<MeshFilter>();
            renderer.material = material;
            filter.mesh = mesh;

            zoneObject.name = "Zone";
            return zoneObject;
        }

        private Mesh GenerateMesh(List<Vector3> verts, List<int> indices)
        {
            Mesh mesh = new Mesh();
            mesh.vertices = verts.ToArray();
            mesh.triangles = indices.ToArray();
            mesh.Optimize();
            return mesh;
        }

        private List<Vector3> ExtractEdgeTiles(Vector3[] tileLocations)
        {
            List<Vector3> edgeTiles = new List<Vector3>();
            int neighborCount = 0;

            for (int i = 0; i < tileLocations.Length; i++)
            {
                neighborCount = 0;
                for (int j = 0; j < tileLocations.Length; j++)
                {
                    if (j == i) continue;
                    if (Approximately((tileLocations[i] - tileLocations[j]).magnitude, 1f))
                    {
                        neighborCount++;
                    }
                }

                if (neighborCount == 4) continue;
                edgeTiles.Add(tileLocations[i]);
            }

            return edgeTiles;
        }

        private List<Vector3> DeterminePermimeterTiles(List<Vector3> edgeTiles, List<Vector3> allLocations)
        {
            List<Vector3> perimeterTiles = new List<Vector3>();

            //Add a tile on every side of the edge tiles
            for(int i = 0; i < edgeTiles.Count; i++)
            {
                perimeterTiles.Add(edgeTiles[i] + Vector3.left);
                perimeterTiles.Add(edgeTiles[i] + Vector3.right);
                perimeterTiles.Add(edgeTiles[i] + Vector3.forward);
                perimeterTiles.Add(edgeTiles[i] + Vector3.back);
            }

            //Remove duplicate locations compared to allLocations
            foreach(Vector3 tile in allLocations)
            {
                for(int i = perimeterTiles.Count - 1; i >= 0; i--)
                {
                    if(Vector3.Distance(tile, perimeterTiles[i]) < (m_tileSize * 0.5f))
                    {
                        perimeterTiles.RemoveAt(i);
                    }
                }
            }
            
            //Remove duplicate locations compared to other perimeter tiles
            for(int i = perimeterTiles.Count - 1; i >= 1; i--)
            {
                for(int j = i - 1; j >= 0; j--)
                {
                    if(Vector3.Distance(perimeterTiles[i], perimeterTiles[j]) < (m_tileSize * 0.5f))
                    {
                        perimeterTiles.RemoveAt(i);
                        break;
                    }
                }
            }

            return perimeterTiles;
        }

        private List<Vector3> CalculateEdgeVertices(List<Vector3> edgeTiles, List<Vector3> perimeterTiles)
        {
            List<Vector3> edgeTileVertices = new List<Vector3>();
            List<int> edgeTileIndices = new List<int>();
            for(int i = 0; i < edgeTiles.Count; i++)
            {
                CalculateTileVerts(edgeTiles[i], ref edgeTileVertices, ref edgeTileIndices);
            }

            List<Vector3> perimeterVertices = new List<Vector3>();
            List<int> perimeterIndices = new List<int>();
            for(int i = 0; i < perimeterTiles.Count; i++)
            {
                CalculateTileVerts(perimeterTiles[i], ref perimeterVertices, ref perimeterIndices);
            }

            //Remove any vertices that do not also appear in the perimeter vertices
            int count = 0;
            for(int i = edgeTileVertices.Count - 1; i >= 0; i--)
            {
                count = 0;
                for(int j = 0; j < perimeterVertices.Count; j++)
                {
                    if(Vector3.Distance(edgeTileVertices[i], perimeterVertices[j]) < (m_tileSize * 0.3f))
                    {
                        count++;
                        break;
                    }
                }

                if(count == 0)
                {
                    edgeTileVertices.RemoveAt(i);
                }
            }

            for(int i = edgeTileVertices.Count - 1; i > 0; i--)
            {
                for(int j = i - 1; j >= 0; j--)
                {
                    if(Vector3.Distance(edgeTileVertices[i], edgeTileVertices[j]) < (m_tileSize * 0.3f))
                    {
                        edgeTileVertices.RemoveAt(i);
                        break;
                    }
                }
            }

            return edgeTileVertices;
        }

        private List<Connection> GenerateConnections(List<Vector3> edgeVertices, List<Vector3> edgeTiles)
        {
            List<Connection> connections = new List<Connection>();

            for(int i = 0; i < edgeVertices.Count; i++)
            {
                Connection connection = new Connection();
                connection.root = edgeVertices[i];

                for (int j = 0; j < edgeTiles.Count; j++)
                {
                    if(Vector3.Distance(edgeTiles[j] + m_tileOffset, connection.root) < (m_tileSize))
                    {
                        connection.sharedTiles.Add(edgeTiles[j] + m_tileOffset);
                    }
                }

                connections.Add(connection);
            }

            //Get all connections regarless of whether they are valid or not.
            for (int i = 0; i < connections.Count; i++)
            {
                Connection node = connections[i];
                node.connections = new List<Connection>();

                for(int j = 0; j < connections.Count; j++)
                {
                    if (j == i) continue;
                    if(Approximately(Vector3.Distance(node.root, connections[j].root), m_tileSize))
                    {
                        node.connections.Add(connections[j]);
                    }
                }
                connections[i] = node;
            }

            //Remove invalid connections
            for(int i = 0; i < connections.Count; i++)
            {
                connections[i].RemoveInvalidConnections();
            }

            return connections;
        }

        private List<ConnectionRing> GenerateConnectionRings(List<Connection> connections)
        {
            List<ConnectionRing> rings = new List<ConnectionRing>();
            List<Connection> intersectionPoints = new List<Connection>();

            //Separate out the intersection points
            for(int i = connections.Count - 1; i >= 0; i--)
            {
                if (connections[i].connections.Count == 2) continue;
                intersectionPoints.Add(connections[i]);
                connections.RemoveAt(i);
            }

            //Create rings
            while (connections.Count > 0)
            {
                ConnectionRing ring = new ConnectionRing();
                CreateRing(connections[0], ref connections, ref ring);
                rings.Add(ring);
            }

            //Connect rings when possible    
            for (int i = rings.Count - 1; i > 0; i--)
            {   
                for(int j = i - 1; j >= 0; j--)
                {
                    if(ConnectionRing.IsJoinable(rings[i], rings[j]))
                    {
                        rings[j].JoinRing(rings[i]);
                        rings.RemoveAt(i);
                        break;
                    }
                }
            }
            
            //Connect broken rings
            for(int i = 0; i < rings.Count; i++)
            {
                if (rings[i].IsClosed) continue;

                for(int j = 0; j < intersectionPoints.Count; j++)
                {
                    if(rings[i].Ring[rings[i].Ring.Count - 1].HasConnection(intersectionPoints[j])
                        && intersectionPoints[j].HasConnection(rings[i].Ring[0]))
                    {
                        rings[i].AddConnection(intersectionPoints[j]);
                        break;
                    }
                }
            }

            return rings;
        }

        private void CreateRing(Connection c, ref List<Connection> availableConnections, ref ConnectionRing ring)
        {
            if (!ring.IsValidConnection(c)) return;
            ring.AddConnection(c);
            availableConnections.Remove(c);

            int availableNode = -1;
            for(int i = 0; i < c.connections.Count; i++)
            {
                if (availableConnections.Contains(c.connections[i]) && c.connections[i].connections.Count == 2)
                {
                    availableNode = i;
                    break;
                }
            }

            if (availableNode == -1) return;
            CreateRing(c.connections[availableNode], ref availableConnections, ref ring);
        }
        #endregion
    }
}
