﻿using Unity.Jobs;
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

    /// <summary>
    /// CalculateTileVerts is a job that calculates
    /// the inner tile verts and indices.
    /// </summary>
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
            vert1.y = tileLocations[index].y;
            vert1.z = tileLocations[index].z + offset;
            vert1.x = tileLocations[index].x + offset;

            Vector3 vert2 = new Vector3();
            vert2.y = tileLocations[index].y;
            vert2.z = tileLocations[index].z - offset;
            vert2.x = tileLocations[index].x + offset;

            Vector3 vert3 = new Vector3();
            vert3.y = tileLocations[index].y;
            vert3.z = tileLocations[index].z -  offset;
            vert3.x = tileLocations[index].x - offset;

            Vector3 vert4 = new Vector3();
            vert4.y = tileLocations[index].y;
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
    
    [BurstCompile(CompileSynchronously = true)]
    public struct FilterEdgePositions : IJobParallelForFilter
    {
        [NativeDisableParallelForRestriction]
        [ReadOnly]
        public NativeArray<Vector3> locations;

        public bool Execute(int index)
        {
            int neighbourCount = 0;
            float magnitude = 0f;

            for (int i = 0; i != locations.Length; i++)
            {
                if (i == index) continue;
                magnitude = (locations[i] - locations[index]).magnitude;

                if (math.abs(1f - magnitude) < 0.01f)
                {
                    neighbourCount++;
                }
            }

            if (neighbourCount == 4) return false;
            return true;
        }
    }

    /// <summary>
    /// GenreatePerimeterTiles is a job that generates a perimeter
    /// tile on all sides of an edge tile.
    /// </summary>
    [BurstCompile(CompileSynchronously = true)]
    public struct GeneratePerimeterTiles : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<Vector3> edgeTiles;

        [NativeDisableParallelForRestriction]
        [ReadOnly]
        public NativeArray<Vector3> allLocations;

        [NativeDisableParallelForRestriction]
        public NativeArray<Vector3> perimeterTiles;

        public float distanceAmount;

        public void Execute(int index)
        {
            int lowBounds = index * 4;
            perimeterTiles[lowBounds] = edgeTiles[index] + Vector3.left;
            perimeterTiles[lowBounds + 1] = edgeTiles[index] + Vector3.right;
            perimeterTiles[lowBounds + 2] = edgeTiles[index] + Vector3.forward;
            perimeterTiles[lowBounds + 3] = edgeTiles[index] + Vector3.back;

            float magnitude = 0f;

            for(int i = 0; i < 4; i++)
            {
                for (int j = 0; j < allLocations.Length; j++)
                {
                    magnitude = (perimeterTiles[lowBounds + i] - allLocations[j]).magnitude;
                    if(magnitude < distanceAmount)
                    {
                        perimeterTiles[lowBounds + i] = Vector3.negativeInfinity;
                        break;
                    }
                }
            }
        }
    }

    [BurstCompile(CompileSynchronously = true)]
    public struct FilterLegalPerimeterTileLocations : IJobParallelForFilter
    {
        [ReadOnly]
        [NativeDisableParallelForRestriction]
        public NativeArray<Vector3> perimeterTiles;

        [ReadOnly]
        public float distanceAmount;

        public bool Execute(int index)
        {
            if(float.IsNegativeInfinity(perimeterTiles[index].x))
            {
                return false;
            }

            float magnitude = 0f;
            for(int i = index + 1; i != perimeterTiles.Length; i++)
            {
                magnitude = (perimeterTiles[index] - perimeterTiles[i]).magnitude;
                if(magnitude < distanceAmount)
                {
                    return false;
                }
            }

            return true;
        }
    }
}