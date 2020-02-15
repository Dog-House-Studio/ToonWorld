using DogHouse.ToonWorld.Animation;
using UnityEngine;

namespace DogHouse.ToonWorld.UI
{
    /// <summary>
    /// FadeCanvasGroup is a script that when attached
    /// to an object with a CanvasGroup will fade that
    /// group when enabled.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeCanvasGroup : MonoBehaviour, IFade
    {
        #region Public Variables
        public float FadeValue => m_group.alpha;
        #endregion

        #region Private Variables
        [SerializeField]
        private Lerpable m_canvasLerp;

        private CanvasGroup m_group;
        private bool m_fadingIn = true;
        #endregion

        #region Main Methods
        private void Start()
        {
            m_group = GetComponent<CanvasGroup>();
        }

        public void FadeOut()
        {
            m_canvasLerp.BeginLerping();
            m_fadingIn = false;
        }

        private void OnEnable()
        {
            m_canvasLerp.BeginLerping();
            m_fadingIn = true;
        }

        public void SetFadeValue(float value)
        {
            m_group.alpha = value;
        }

        private void Update()
        {
            if (!m_canvasLerp.IsLerping) return;
            m_canvasLerp.Update();

            if(m_fadingIn) SetFadeValue(m_canvasLerp.LerpValue);
            if (!m_fadingIn)
            {
                SetFadeValue(1f - m_canvasLerp.LerpValue);
                if (!m_canvasLerp.IsLerping) gameObject.SetActive(false);
            }

        }
        #endregion
    }
}
