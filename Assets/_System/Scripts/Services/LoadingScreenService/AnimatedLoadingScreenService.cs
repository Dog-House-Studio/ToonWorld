using DogScaffold;
using DogHouse.ToonWorld.UI;

namespace DogHouse.CoreServices
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
        #endregion

        #region Main Methods
        void Awake()
        {
            m_loadingScreen = GetComponentInChildren<ILoadingScreen>();
        }

        public void SetDisplay(bool value)
        {
            if(value)
            {
                m_loadingScreen?.TransitionIn();
                return;
            }

            m_loadingScreen?.TransitionOut();
        }
        #endregion
    }
}
