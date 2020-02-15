using System.Collections.Generic;
using UnityEngine;
using DogScaffold;
using static UnityEngine.Mathf;

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
        #endregion
    }
}
