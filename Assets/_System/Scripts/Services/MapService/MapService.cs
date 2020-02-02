using UnityEngine;
using DogScaffold;
using DogHouse.ToonWorld.Map;
using System.Linq;

namespace DogHouse.ToonWorld.Services
{
    /// <summary>
    /// MapService is responsible for getting the
    /// generator, generating the map, and operating
    /// on the resulting node web.
    /// </summary>
    public class MapService : BaseService<IMapService>, IMapService
    {
        #region Private Variables
        private NodeWeb m_nodeWeb;
        #endregion

        #region Main Methods
        void Start()
        {
            IOverworldMapGenerator generator;
            generator = FindObjectsOfType<MonoBehaviour>().OfType<IOverworldMapGenerator>().FirstOrDefault();
            m_nodeWeb = generator.Generate();

            m_nodeWeb.Start.SetAsCurrent();
        }
        #endregion
    }
}
