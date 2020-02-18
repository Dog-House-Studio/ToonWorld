using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;
using Unity.Mathematics;
using static Unity.Mathematics.math;

namespace DogHouse.ToonWorld.Services
{
    /*
     * This script contains various jobs that
     * the mesh zone service uses for the sorting
     * and creation of the zones.
     */
    [BurstCompile]
    public struct CalculateTileVerts : IJobParallelFor
    {
        public NativeArray<float3> vertices;
        public NativeArray<float3> tileLocations;
        public NativeArray<int> indices;
        public float offset;

        public void Execute(int index)
        {
            int vertIndex = index * 4;
            int indiceIndex = index * 6;

            float3 vert1 = tileLocations[index];
            vert1.z += offset;
            vert1.x += offset;

            float3 vert2 = tileLocations[index];
            vert1.z -= offset;
            vert1.x += offset;

            float3 vert3 = tileLocations[index];
            vert1.z -= offset;
            vert1.x -= offset;

            float3 vert4 = tileLocations[index];
            vert1.z += offset;
            vert1.x -= offset;

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
