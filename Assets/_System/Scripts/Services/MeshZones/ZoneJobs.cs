using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

namespace DogHouse.ToonWorld.Services
{
    /*
     * This script contains various jobs that
     * the mesh zone service uses for the sorting
     * and creation of the zones.
     */
    [BurstCompile(CompileSynchronously = true)]
    public struct CalculateTileVerts : IJobParallelFor
    {
        [NativeDisableParallelForRestriction]
        public NativeArray<Vector3> vertices;

        [ReadOnly]
        public NativeArray<Vector3> tileLocations;

        [NativeDisableParallelForRestriction]
        public NativeArray<int> indices;

        [ReadOnly]
        public float offset;

        public void Execute(int index)
        {
            int vertIndex = index * 4;
            int indiceIndex = index * 6;

            Vector3 vert1 = new Vector3();
            vert1.z = tileLocations[index].z + offset;
            vert1.x = tileLocations[index].x + offset;

            Vector3 vert2 = new Vector3();
            vert2.z = tileLocations[index].z - offset;
            vert2.x = tileLocations[index].x + offset;

            Vector3 vert3 = new Vector3();
            vert3.z = tileLocations[index].z -  offset;
            vert3.x = tileLocations[index].x - offset;

            Vector3 vert4 = new Vector3();
            vert4.z = tileLocations[index].z + offset;
            vert4.x = tileLocations[index].x - offset;

            vertices[vertIndex] = vert1;
            vertices[vertIndex + 1] = vert2;
            vertices[vertIndex + 2] = vert3;
            vertices[vertIndex + 3] = vert4;

            indices[indiceIndex] = vertIndex;
            indices[indiceIndex + 1] = vertIndex + 1;
            indices[indiceIndex + 2] = vertIndex + 2;

            indices[indiceIndex + 3] = vertIndex;
            indices[indiceIndex + 4] = vertIndex + 2;
            indices[indiceIndex + 5] = vertIndex + 3;
        }
    }
}
