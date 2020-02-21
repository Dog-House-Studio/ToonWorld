using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

namespace DogHouse.ToonWorld.Jobs
{
    /// <summary>
    /// ApplyNativeListFilter is a job that given
    /// an index filter will apply the filter.
    /// </summary>
    [BurstCompile]
    public struct ApplyNativeListFilterVector3 : IJob
    {
        [ReadOnly]
        public NativeArray<Vector3> originalCollection;

        [ReadOnly]
        public NativeList<int> indexCollection;

        [WriteOnly]
        public NativeList<Vector3> filteredList;

        public void Execute()
        {
            for(int i = 0; i != indexCollection.Length; i++)
            {
                filteredList.Add(originalCollection[indexCollection[i]]);
            }
        }
    }
}
