using UnityEngine;
using UnityEngine.UI;
using DogScaffold;
using DogHouse.ToonWorld.Services;

namespace DogHouse.ToonWorld.Map
{
    /// <summary>
    /// ReturnToMapButton is a script that interfaces
    /// with the map service to return to the map scene.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class ReturnToMapButton : MonoBehaviour
    {
        #region Private Variables
        private Button m_button;
        private ServiceReference<IMapService> m_mapService 
            = new ServiceReference<IMapService>();
        #endregion

        #region Main Methods
        private void OnEnable()
        {
            m_button = GetComponent<Button>();
            m_button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            m_button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            if (!m_mapService.CheckServiceRegistered()) return;
            m_mapService.Reference.ReturnToMapScene();
        }
        #endregion
    }
}
