using DogScaffold;
using UnityEngine;

namespace DogHouse.ToonWorld.Services
{
    /// <summary>
    /// IMeshZoneService is an interface that describes
    /// a service which can be used to generate a mesh 
    /// that shows a particular zone on the battlefield.
    /// </summary>
    public interface IMeshZoneService : IService
    {
        GameObject GenerateZone(Vector3[] tileLocations);
    }
}
