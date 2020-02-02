using DogScaffold;
using DogHouse.ToonWorld.Map;

namespace DogHouse.ToonWorld.Services
{
    /// <summary>
    /// IMapService is an interface that describes a
    /// map service. A map service is responsible for 
    /// keeping track of a node web and opperating
    /// on it.
    /// </summary>
    public interface IMapService : IService
    {
        void ReportIconSelected(Node icon);
        void ReturnToMapView();

    }
}
