using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DogScaffold;
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
        #endregion

        #region Main Methods
        public GameObject GenerateZone(Vector3[] tileLocations)
        {
            GameObject zoneObject = SetupZoneObject();

            List<Vector3> verts = new List<Vector3>();
            List<int> indices = new List<int>();

            GenerateZoneVerts(tileLocations, ref verts, ref indices);

            Mesh mesh = new Mesh();
            mesh.vertices = verts.ToArray();
            mesh.triangles = indices.ToArray();
            mesh.Optimize();

            zoneObject.GetComponent<MeshFilter>().mesh = mesh;
            zoneObject.GetComponent<MeshRenderer>().material = m_zoneMaterial;

            return zoneObject;
        }

        private void GenerateZoneVerts(Vector3[] tileLocations, 
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

            location += m_tileOffset;

            Vector3 vert1 = location;
            vert1.y += 0.5f * m_tileSize;
            vert1.x += 0.5f * m_tileSize;

            Vector3 vert2 = location;
            vert2.y -= 0.5f * m_tileSize;
            vert2.x += 0.5f * m_tileSize;

            Vector3 vert3 = location;
            vert3.y -= 0.5f * m_tileSize;
            vert3.x -= 0.5f * m_tileSize;

            Vector3 vert4 = location;
            vert4.y += 0.5f * m_tileSize;
            vert4.x -= 0.5f * m_tileSize;

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
        private GameObject SetupZoneObject()
        {
            GameObject zoneObject = Instantiate(new GameObject());
            zoneObject.transform.SetParent(transform);
            zoneObject.transform.localPosition = Vector3.zero;
            zoneObject.transform.localRotation = Quaternion.identity;
            zoneObject.AddComponent<MeshRenderer>();
            zoneObject.AddComponent<MeshFilter>();
            zoneObject.name = "Zone";
            return zoneObject;
        }
        #endregion
    }
}
