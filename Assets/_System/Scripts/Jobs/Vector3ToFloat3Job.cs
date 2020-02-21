using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;

namespace DogHouse.ToonWorld.Jobs
{
    /// <summary>
    /// Vector3ToFloat3Job is a parallel job that converts
    /// a vector3 to a float 3.
    /// </summary>
    [BurstCompile(CompileSynchronously = true)]
    public class Vector3ToFloat3Job : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<Vector3> input;

        [WriteOnly]
        public NativeArray<float3> output;

        public void Execute(int index)
        {
            output[index] = new float3(input[index].x, input[index].y, input[index].z);
        }
    }
}
