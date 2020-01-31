using DogScaffold;
using DogHouse.ToonWorld.UI;
using System;
using DogHouse.CoreServices;

namespace DogHouse.ToonWorld.Services
{
    /// <summary>
    /// The AnimatedLoadingScreenService is a script that 
    /// implements the ILoadingScreenService. This implementation
    /// takes advantage of the ILoadingScreen interface of a 
    /// child object.
    /// </summary>
    public class AnimatedLoadingScreenService : BaseService<ILoadingScreenService>, 
        ILoadingScreenService
    {
        #region Private Variables
        private ILoadingScreen m_loadingScreen;

        private ServiceReference<ILogService> m_logService 
            = new ServiceReference<ILogService>();
        #endregion

        #region Main Methods
        void Awake()
        {
            m_loadingScreen = GetComponentInChildren<ILoadingScreen>();
        }

        public void TransitionIn(Action callback = null)
        {
            if(m_loadingScreen == null)
            {
                m_logService.Reference?.LogError("No Loading Screen asset setup");
                return;
            }

            m_loadingScreen?.TransitionIn(callback);
        }

        public void TransitionOut(Action callback = null)
        {
            if (m_loadingScreen == null)
            {
                m_logService.Reference?.LogError("No Loading Screen asset setup");
                return;
            }

            m_loadingScreen?.TransitionOut(callback);
        }
        #endregion
    }
}
