namespace DogHouse.ToonWorld.Map
{
    /// <summary>
    /// IOverworldMapGenerator is an interface that defines
    /// the way we interact with map generators. A script that
    /// implements this interface is responsible for generating
    /// the overworld map for the current act.
    /// </summary>
    public interface IOverworldMapGenerator
    {
        void SetSeed(int seedValue);
        void Generate();
        void Display(bool value);
    }
}
