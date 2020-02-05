using UnityEngine;
using DogScaffold;
using DogHouse.ToonWorld.Map;
using System.Linq;
using Cinemachine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
        private Camera m_mapSceneCamera;

        [SerializeField]
        private GameObject m_UIObject;

        [SerializeField]
        private Button m_goButton;

        [SerializeField]
        private Button m_backButton;

        [SerializeField]
        private TMP_Text m_text;

        private NodeWeb m_nodeWeb;
        private Node m_currentNode = null;
        private Node m_zoomedNode = null;

        private ServiceReference<IGameSceneManagerService> m_sceneManager 
            = new ServiceReference<IGameSceneManagerService>();

        private ServiceReference<ILoadingScreenService> m_loadingScreenService 
            = new ServiceReference<ILoadingScreenService>();
        #endregion

        #region Main Methods
        void Start()
        {
            IOverworldMapGenerator generator;
            generator = FindObjectsOfType<MonoBehaviour>().OfType<IOverworldMapGenerator>().FirstOrDefault();
            m_nodeWeb = generator.Generate();


            //m_nodeWeb.Start.SetAsCurrent(true);
            m_currentNode = m_nodeWeb.Start;
        }

        public override void OnEnable()
        {
            base.OnEnable();
            m_backButton.onClick.AddListener(ReturnToMapView);
            m_goButton.onClick.AddListener(GoToNodeScene);
        }

        public override void OnDisable()
        {
            base.OnDisable();
            m_backButton.onClick.RemoveAllListeners();
            m_goButton.onClick.RemoveAllListeners();
        }

        public void ReportIconSelected(Node icon)
        {
            m_zoomedNode = icon;
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

            m_text.text = icon.IconType.LocationName.ToUpper();
            
        }

        public void ReturnToMapView()
        {
            m_iconCamera.gameObject.SetActive(false);
            m_mapCamera.gameObject.SetActive(true);

            m_UIObject?.SetActive(false);
            m_currentNode.SetAsCurrent(true);
        }

        private void GoToNodeScene()
        {
            if (!m_sceneManager.CheckServiceRegistered()) return;
            m_sceneManager.Reference.LoadScene(m_zoomedNode.IconType.SceneDefinition, 
                OnIconSceneLoaded);
        }

        private void OnIconSceneLoaded()
        {
            m_mapSceneCamera.gameObject?.SetActive(false);
            m_UIObject.gameObject?.SetActive(false);
        }

        public void ReturnToMapScene(string currentSceneName)
        {
            m_loadingScreenService.Reference.TransitionIn(
                ()=> { LoadingScreenFadedIn(currentSceneName); });
        }

        private void LoadingScreenFadedIn(string sceneName)
        {
            m_mapSceneCamera.gameObject.SetActive(true);
            m_UIObject.gameObject.SetActive(true);
            SceneManager.UnloadScene(sceneName);
            m_loadingScreenService.Reference.TransitionOut();
            ReturnToMapView();
        }
        #endregion
    }
}
