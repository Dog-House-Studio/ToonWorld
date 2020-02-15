using System.Collections.Generic;
using UnityEngine;
using DogScaffold;
using static UnityEngine.Mathf;
using System;
using System.Linq;

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
            Debug.Log(edgeTiles.Count);
            List<Vector3> perimeterTiles = DeterminePermimeterTiles(edgeTiles, tileLocations.ToList());
            Debug.Log(perimeterTiles.Count);

            List<Vector3> edgeVertices = CalculateEdgeVertices(edgeTiles, perimeterTiles);

            foreach(Vector3 vert in edgeVertices)
            {
                Instantiate(m_cubePrefab, vert, Quaternion.identity);
            }

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

            return edgeTileVertices;
        }
        #endregion
    }
}
