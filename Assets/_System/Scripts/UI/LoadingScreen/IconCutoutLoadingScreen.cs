using System;
using UnityEngine;

namespace DogHouse.ToonWorld.UI
{
    /// <summary>
    /// IconCutoutLoadingScreen is an implementation
    /// of the ILoadingScreen interface. The Icon Cutout
    /// effect uses a mask to transition in the loading
    /// screen.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class IconCutoutLoadingScreen : MonoBehaviour, ILoadingScreen
    {
        #region Public Variables
        public Action OnTransitionedIn { get; set; }
        public Action OnTransitionedOut { get; set; }
        #endregion

        #region Private Variables
        private Animator m_animator;
        private LoadingScreenState m_state = LoadingScreenState.OFF;
        #endregion

        #region Main Methods
        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        public void TransitionIn()
        {
            
        }

        public void TransitionIn(Action callback)
        {
            
        }

        public void TransitionOut()
        {
            
        }

        public void TransitionOut(Action callback)
        {
            
        }
        #endregion
    }
}
