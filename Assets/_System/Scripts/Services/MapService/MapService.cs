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

        [SerializeField]
        private GameObject m_UIObject;

        private NodeWeb m_nodeWeb;
        private Node m_currentNode = null;
        #endregion

        #region Main Methods
        void Start()
        {
            IOverworldMapGenerator generator;
            generator = FindObjectsOfType<MonoBehaviour>().OfType<IOverworldMapGenerator>().FirstOrDefault();
            m_nodeWeb = generator.Generate();

            m_nodeWeb.Start.SetAsCurrent(true);
            m_currentNode = m_nodeWeb.Start;
        }

        public void ReportIconSelected(Node icon)
        {
            m_currentNode.SetAsCurrent(false);
            icon.SetIconSelectedColor(true);

            m_iconCamera.transform.position 
                = icon.m_visualController.m_cameraPositionTarget
                .transform.position;

            m_iconCamera.LookAt = icon.m_visualController
                .m_cameraLookAtTarget.transform;

            m_iconCamera.gameObject.SetActive(true);
            m_mapCamera.gameObject.SetActive(false);

            m_UIObject?.SetActive(true);
        }

        public void ReturnToMapView()
        {
            m_iconCamera.gameObject.SetActive(false);
            m_mapCamera.gameObject.SetActive(true);
        }
        #endregion
    }
}
