using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using Unity.Mathematics;

namespace DogHouse.ToonWorld.Jobs
{
    /// <summary>
    /// Float3ToVector3Job is a job that converts an
    /// array of float3 to an array of vector3.
    /// </summary>
    [BurstCompile(CompileSynchronously = true)]
    public struct Float3ToVector3Job : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<float3> input;

        [WriteOnly]
        public NativeArray<Vector3> output;

        public void Execute(int index)
        {
            output[index] = new Vector3(input[index].x, input[index].y, input[index].z);
        }
    }
}
