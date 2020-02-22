using System.Collections.Generic;
using UnityEngine;
using DogScaffold;
using static UnityEngine.Mathf;
using System.Linq;
using System;
using System.Diagnostics;
using Unity.Jobs;
using Unity.Collections;
using DogHouse.ToonWorld.Jobs;
using Unity.Mathematics;

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
            Vector3[] insideVerts = new Vector3[tileLocations.Length * 4];
            int[] insideIndices = new int[tileLocations.Length * 6];
            GenerateInsideMeshData(tileLocations, ref insideVerts, ref insideIndices);
            Mesh insideMesh = GenerateMesh(insideVerts, insideIndices);

            List<Vector3> edgeVerts = new List<Vector3>();
            List<int> edgeIndices = new List<int>();

            GenerateEdgeMeshData(tileLocations, ref edgeVerts, ref edgeIndices, edgeVerts);

            Mesh edgeMesh = GenerateMesh(edgeVerts, edgeIndices);

            return SetupZoneObject(insideMesh, m_zoneMaterial);
        }

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
            mesh.SetVertices(verts);
            mesh.SetTriangles(indices, 0);
            return mesh;
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

            for (int i = 0; i < m_rings.Count; i++)
            {
                if (i == 1) Gizmos.color = Color.green;
                if (i == 2) Gizmos.color = Color.blue;
                if (i == 3) Gizmos.color = Color.gray;
                if (i == 4) Gizmos.color = Color.cyan;

                for (int j = 0; j < m_rings[i].Ring.Count; j++)
                {
                    Gizmos.DrawLine(m_rings[i].Ring[j].root + Vector3.up * (0.1f * i), m_rings[i].Ring[(j + 1) % m_rings[i].Ring.Count].root + Vector3.up * (0.1f * i));
                    UnityEditor.Handles.Label(m_rings[i].Ring[j].root + Vector3.up * (0.1f * i), j.ToString());
                }
            }
        }
        #endregion

        #region Utility Methods
        private Mesh GenerateMesh(Vector3[] verts, int[] indices)
        {
            Mesh mesh = new Mesh();
            mesh.SetVertices(verts);
            mesh.SetTriangles(indices, 0);
            return mesh;
        }

        private List<Vector3> CalculateEdgeVertices(Vector3[] edgeTiles, Vector3[] perimeterTiles)
        {
            List<Vector3> edgeTileVertices = new List<Vector3>();
            List<int> edgeTileIndices = new List<int>();
            for (int i = 0; i < edgeTiles.Length; i++)
            {
                CalculateTileVerts(edgeTiles[i], ref edgeTileVertices, ref edgeTileIndices);
            }

            List<Vector3> perimeterVertices = new List<Vector3>();
            List<int> perimeterIndices = new List<int>();
            for (int i = 0; i < perimeterTiles.Length; i++)
            {
                CalculateTileVerts(perimeterTiles[i], ref perimeterVertices, ref perimeterIndices);
            }

            //Remove any vertices that do not also appear in the perimeter vertices
            int count = 0;
            for (int i = edgeTileVertices.Count - 1; i >= 0; i--)
            {
                count = 0;
                for (int j = 0; j < perimeterVertices.Count; j++)
                {
                    if (Vector3.Distance(edgeTileVertices[i], perimeterVertices[j]) < (m_tileSize * 0.3f))
                    {
                        count++;
                        break;
                    }
                }

                if (count == 0)
                {
                    edgeTileVertices.RemoveAt(i);
                }
            }

            for (int i = edgeTileVertices.Count - 1; i > 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (Vector3.Distance(edgeTileVertices[i], edgeTileVertices[j]) < (m_tileSize * 0.3f))
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

            for (int i = 0; i < edgeVertices.Count; i++)
            {
                Connection connection = new Connection();
                connection.root = edgeVertices[i];

                for (int j = 0; j < edgeTiles.Count; j++)
                {
                    if (Vector3.Distance(edgeTiles[j] + m_tileOffset, connection.root) < (m_tileSize))
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

                for (int j = 0; j < connections.Count; j++)
                {
                    if (j == i) continue;
                    if (Approximately(Vector3.Distance(node.root, connections[j].root), m_tileSize))
                    {
                        node.connections.Add(connections[j]);
                    }
                }
                connections[i] = node;
            }

            //Remove invalid connections
            for (int i = 0; i < connections.Count; i++)
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
            for (int i = connections.Count - 1; i >= 0; i--)
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
                for (int j = i - 1; j >= 0; j--)
                {
                    if (ConnectionRing.IsJoinable(rings[i], rings[j]))
                    {
                        rings[j].JoinRing(rings[i]);
                        rings.RemoveAt(i);
                        break;
                    }
                }
            }

            //Connect broken rings
            for (int i = 0; i < rings.Count; i++)
            {
                if (rings[i].IsClosed) continue;

                for (int j = 0; j < intersectionPoints.Count; j++)
                {
                    if (rings[i].Ring[rings[i].Ring.Count - 1].HasConnection(intersectionPoints[j])
                        && intersectionPoints[j].HasConnection(rings[i].Ring[0]))
                    {
                        rings[i].AddConnection(intersectionPoints[j]);
                        break;
                    }
                }
            }

            //Merge rings
            for (int i = rings.Count - 1; i > 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (ConnectionRing.IsMergable(rings[i], rings[j]))
                    {
                        rings[j].MergeRing(rings[i]);
                        rings.RemoveAt(i);
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
            for (int i = 0; i < c.connections.Count; i++)
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

        private List<List<Vector3>> SeparateTileLocations(Vector3[] tileLocations)
        {
            List<List<Vector3>> result = new List<List<Vector3>>();
            for (int i = 0; i < tileLocations.Length; i++)
            {
                for (int j = 0; j <= result.Count; j++)
                {
                    if (j == result.Count)
                    {
                        result.Add(new List<Vector3>());
                        result[j].Add(tileLocations[i]);
                        break;
                    }

                    if (Approximately(result[j][0].y, tileLocations[i].y))
                    {
                        result[j].Add(tileLocations[i]);
                        break;
                    }
                }
            }
            return result;
        }

        private Vector3[] ApplyPositionOffset(Vector3[] vecArray)
        {
            NativeArray<Vector3> vec = new NativeArray<Vector3>(vecArray, Allocator.TempJob);
            var job = new ApplyOffsetToVector3ArrayJob()
            {
                vecArray = vec,
                offset = m_tileOffset
            };

            JobHandle handle = job.Schedule(vecArray.Length, 8);
            handle.Complete();

            vec.CopyTo(vecArray);
            vec.Dispose();
            return vecArray;
        }
        #endregion

        #region Inside Logic
        private void GenerateInsideMeshData(Vector3[] tileLocations,
            ref Vector3[] verts, ref int[] indices)
        {
            NativeArray<Vector3> vec3_vertices = new NativeArray<Vector3>(tileLocations.Length * 4, Allocator.TempJob);
            NativeArray<Vector3> vec3_tilePosition = new NativeArray<Vector3>(tileLocations, Allocator.TempJob);
            NativeArray<int> indexes = new NativeArray<int>(tileLocations.Length * 6, Allocator.TempJob);

            var offsetJob = new ApplyOffsetToVector3ArrayJob()
            {
                vecArray = vec3_tilePosition,
                offset = m_tileOffset
            };

            JobHandle offsetHandle = offsetJob.Schedule(vec3_tilePosition.Length, 8);
            offsetHandle.Complete();

            var job = new CalculateTileVerts()
            {
                vertices = vec3_vertices,
                tileLocations = vec3_tilePosition,
                indices = indexes,
                offset = 0.5f * m_tileSize
            };

            JobHandle jobHandle = job.Schedule(tileLocations.Length, 8);
            jobHandle.Complete();

            vec3_vertices.CopyTo(verts);
            indexes.CopyTo(indices);

            vec3_vertices.Dispose();
            vec3_tilePosition.Dispose();
            indexes.Dispose();
        }
        #endregion

        #region Edge Logic

        private void GenerateEdgeMeshData(Vector3[] tileLocations, ref List<Vector3> edgeVerts, ref List<int> edgeIndices, List<Vector3> insideVerts)
        {
            m_rings.Clear();
            List<List<Vector3>> separatedTileLocations = SeparateTileLocations(tileLocations);

            for (int j = 0; j < separatedTileLocations.Count; j++)
            {
                Vector3[] locations = separatedTileLocations[j].ToArray();
                locations = ApplyPositionOffset(locations);

                Vector3[] edgeTiles = ExtractEdgeTiles(locations);

                //float startTime = Time.realtimeSinceStartup;
                //Vector3[] perimeterTiles = DeterminePermimeterTiles(edgeTiles, locations);
                //UnityEngine.Debug.Log((Time.realtimeSinceStartup - startTime) * 1000f);
            
                //List<Vector3> edgeVertices = CalculateEdgeVertices(edgeTiles, perimeterTiles);
                //List<Connection> connections = GenerateConnections(edgeVertices, separatedTileLocations[j]);

                #if UNITY_EDITOR
                for(int i = 0; i < edgeTiles.Length; i++)
                {
                    GameObject point = Instantiate(m_cubePrefab);
                    point.transform.position = edgeTiles[i];
                }

                /*
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
                */
                #endif

                //m_rings.AddRange(GenerateConnectionRings(connections));
            }
        }

        private Vector3[] ExtractEdgeTiles(Vector3[] tileLocations)
        {
            NativeArray<Vector3> locations = new NativeArray<Vector3>(tileLocations, Allocator.TempJob);            
            NativeList<int> edgeIndexes = new NativeList<int>(tileLocations.Length, Allocator.TempJob);

            var initJob = new InitializeSequentialIntNativeList()
            {
                input = edgeIndexes,
                count = tileLocations.Length
            };

            JobHandle initHandle = initJob.Schedule();

            var job = new FilterEdgePositions()
            {
                locations = locations,
            };

            JobHandle handle = job.ScheduleFilter(edgeIndexes, 8, initHandle);
            handle.Complete();

            NativeList<Vector3> edgeLocations = new NativeList<Vector3>(edgeIndexes.Length, Allocator.TempJob);

            var filterJob = new ApplyNativeListFilterVector3()
            {
                originalCollection = locations,
                indexCollection = edgeIndexes,
                filteredList = edgeLocations
            };

            JobHandle applyFilterHandle = filterJob.Schedule();
            applyFilterHandle.Complete();

            Vector3[] edge = new Vector3[edgeLocations.Length];
            edge = edgeLocations.ToArray();
            
            locations.Dispose();
            edgeLocations.Dispose();
            edgeIndexes.Dispose();
            return edge;
        }

        private Vector3[] DeterminePermimeterTiles(Vector3[] edgeTiles, Vector3[] allLocations)
        {
            NativeArray<Vector3> _tileLocations = new NativeArray<Vector3>(allLocations, Allocator.TempJob);
            NativeArray<Vector3> _perimeterTiles = new NativeArray<Vector3>(allLocations.Length * 4, Allocator.TempJob);
            NativeArray<Vector3> _edgeTiles = new NativeArray<Vector3>(edgeTiles, Allocator.TempJob);
            NativeList<int> _resultFilter = new NativeList<int>(allLocations.Length * 4, Allocator.TempJob);

            var job_setupFilter = new InitializeSequentialIntNativeList()
            {
                input = _resultFilter,
                count = allLocations.Length * 4
            };

            JobHandle handle_filterSetup = job_setupFilter.Schedule();

            var job_generateTiles = new GeneratePerimeterTiles()
            {
                edgeTiles = _edgeTiles,
                allLocations = _tileLocations,
                perimeterTiles = _perimeterTiles,
                distanceAmount = m_tileSize * 0.5f
            };

            JobHandle handle_generateTiles = job_generateTiles.Schedule(edgeTiles.Length, 32);
            JobHandle handle_Dependency = JobHandle.CombineDependencies(handle_filterSetup, handle_generateTiles);
            

            var job_filterPerimeter = new FilterLegalPerimeterTileLocations()
            {
                perimeterTiles = _perimeterTiles,
                distanceAmount = m_tileSize * 0.5f
            };

            JobHandle handle_filterPerimeter 
                = job_filterPerimeter.ScheduleFilter(_resultFilter, 16, handle_Dependency);

            NativeList<Vector3> _resultList 
                = new NativeList<Vector3>(allLocations.Length * 4, Allocator.TempJob);

            var job_ApplyFilter = new ApplyNativeListFilterVector3_Deferred()
            {
                originalCollection = _perimeterTiles,
                indexCollection = _resultFilter.AsDeferredJobArray(),
                filteredList = _resultList
            };

            JobHandle handle_ApplyFilter = job_ApplyFilter.Schedule(handle_filterPerimeter);
            handle_ApplyFilter.Complete();

            Vector3[] resultArray = new Vector3[_resultList.Length];
            resultArray = _resultList.ToArray();

            _resultFilter.Dispose();
            _resultList.Dispose();
            _tileLocations.Dispose();
            _perimeterTiles.Dispose();
            _edgeTiles.Dispose();

            return resultArray;
        }
        #endregion
    }
}
