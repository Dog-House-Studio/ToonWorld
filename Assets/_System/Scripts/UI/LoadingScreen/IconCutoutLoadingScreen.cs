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
    [RequireComponent(typeof(AudioSource))]
    public class IconCutoutLoadingScreen : MonoBehaviour, ILoadingScreen
    {
        #region Public Variables
        public Action OnTransitionedIn { get; set; }
        public Action OnTransitionedOut { get; set; }
        #endregion

        #region Private Variables
        [SerializeField]
        private GameObject m_maskObject;

        [SerializeField]
        private AudioClip m_sfxClip;

        private Animator m_animator;
        private AudioSource m_audioSource;
        private LoadingScreenState m_state = LoadingScreenState.OFF;
        private LoadingScreenState m_targetState;

        private Action m_callbackIn;
        private Action m_callbackOut;

        private const string ON_PARAM = "On";
        private const string IDLE_ON = "IdleOn";
        private const string IDLE_OFF = "IdleOff";
        #endregion

        #region Main Methods
        private void Awake()
        {
            m_animator = GetComponent<Animator>();
            m_audioSource = GetComponent<AudioSource>();
        }

        public void TransitionIn()
        {
            if (m_state == LoadingScreenState.TRANSITION_IN) return;
            if (m_state == LoadingScreenState.ON) return;
            m_targetState = LoadingScreenState.ON;
            m_state = LoadingScreenState.TRANSITION_IN;
            m_maskObject?.SetActive(true);
            SetAnimatorValue(true);
            m_audioSource?.PlayOneShot(m_sfxClip);
        }

        public void TransitionIn(Action callback)
        {
            m_callbackIn -= callback;
            m_callbackIn += callback;
            TransitionIn();
        }

        public void TransitionOut()
        {
            if (m_state == LoadingScreenState.TRANSITION_OUT) return;
            if (m_state == LoadingScreenState.OFF) return;
            m_targetState = LoadingScreenState.OFF;
            m_state = LoadingScreenState.TRANSITION_OUT;
            m_maskObject?.SetActive(true);
            SetAnimatorValue(false);
        }

        public void TransitionOut(Action callback)
        {
            m_callbackOut += callback;
            TransitionOut();
        }

        void Update()
        {
            if (m_state == m_targetState) return;

            if(m_state == LoadingScreenState.TRANSITION_IN &&
                m_animator.GetCurrentAnimatorStateInfo(0).IsName(IDLE_ON))
            {
                SetLoadingState(LoadingScreenState.ON);
                return;
            }

            if (m_state == LoadingScreenState.TRANSITION_OUT &&
                m_animator.GetCurrentAnimatorStateInfo(0).IsName(IDLE_OFF))
            {
                SetLoadingState(LoadingScreenState.OFF);
                return;
            }
        }
        #endregion

        #region Utility Methods
        private void SetAnimatorValue(bool value)
        {
            m_animator?.SetBool(ON_PARAM, value);
        }

        private void SetLoadingState(LoadingScreenState state)
        {
            m_state = state;
            if(m_state == LoadingScreenState.ON)
            {
                m_callbackIn?.Invoke();
                m_callbackIn = null;
                OnTransitionedIn?.Invoke();
                return;
            }

            if (m_state == LoadingScreenState.OFF)
            {
                m_callbackOut?.Invoke();
                m_callbackOut = null;
                OnTransitionedOut?.Invoke();
                m_maskObject?.SetActive(false);
                return;
            }
        }
        #endregion
    }
}
