using UnityEngine;
using DogScaffold;
using DogHouse.CoreServices;

namespace DogHouse.ToonWorld.UI
{
    /// <summary>
    /// FindWorldSpaceCamera is a script that finds the 
    /// a camera for the world space canvas.
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class FindWorldSpaceCanvasCamera : MonoBehaviour
    {
        #region Private Variables
        private ServiceReference<ICameraFinder> m_cameraFindService 
            = new ServiceReference<ICameraFinder>();
        #endregion

        #region Main Methods
        private void OnEnable()
        {
            m_cameraFindService.AddRegistrationHandle(HandleCameraFinderAvailable);
        }

        private void OnDisable()
        {
            if (m_cameraFindService.CheckServiceRegistered()) return;
            m_cameraFindService.Reference.OnNewCameraFound -= SetCamera;
        }

        private void HandleCameraFinderAvailable()
        {
            if(m_cameraFindService.Reference.Camera != null)
            {
                SetCamera(m_cameraFindService.Reference.Camera);
                return;
            }

            m_cameraFindService.Reference.OnNewCameraFound -= SetCamera;
            m_cameraFindService.Reference.OnNewCameraFound += SetCamera;
        }

        private void SetCamera(Camera camera)
        {
            Canvas canvas = GetComponent<Canvas>();
            canvas.worldCamera = camera;
        }
        #endregion
    }
}
