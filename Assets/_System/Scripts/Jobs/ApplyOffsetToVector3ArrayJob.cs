using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

namespace DogHouse.ToonWorld.Jobs
{
    /// <summary>
    /// ApplyOffsetToVector3ArrayJob is a job that applys some
    /// vector3 offset to an array of vector3. 
    /// </summary>
    [BurstCompile(CompileSynchronously = true)]
    public struct ApplyOffsetToVector3ArrayJob : IJobParallelFor
    {
        public NativeArray<Vector3> vecArray;
        public Vector3 offset;

        public void Execute(int index)
        {
            vecArray[index] += offset;
        }
    }
}
