using UnityEngine;
using DogScaffold;
using DogHouse.ToonWorld.Map;
using System.Linq;
using Cinemachine;

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
        [SerializeField]
        private CinemachineVirtualCamera m_iconCamera;

        [SerializeField]
        private CinemachineVirtualCamera m_mapCamera;

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

        public void ReportIconSelected(Node icon)
        {
            m_iconCamera.transform.position 
                = icon.m_visualController.m_cameraPositionTarget
                .transform.position;

            m_iconCamera.LookAt = icon.m_visualController
                .m_cameraLookAtTarget.transform;

            m_iconCamera.gameObject.SetActive(true);
            m_mapCamera.gameObject.SetActive(false);
        }

        public void ReturnToMapView()
        {
            m_iconCamera.gameObject.SetActive(false);
            m_mapCamera.gameObject.SetActive(true);
        }
        #endregion
    }
}
