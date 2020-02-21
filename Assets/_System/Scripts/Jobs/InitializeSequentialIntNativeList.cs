using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;

namespace DogHouse.ToonWorld.Jobs
{
    /// <summary>
    /// InitializeSequentialIntNativeList is a job that takes a
    /// NativeList<int> and sets each index equal to the index
    /// value.
    /// </summary>
    [BurstCompile(CompileSynchronously = true)]
    public struct InitializeSequentialIntNativeList : IJob
    {
        public NativeList<int> input;
        public int count;

        public void Execute()
        {
            for(int i = 0; i != count; i++)
            {
                input.Add(i);
            }
        }
    }
}
