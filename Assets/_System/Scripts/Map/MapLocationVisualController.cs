using UnityEngine;
using System;
using DogScaffold;
using DogHouse.CoreServices;

namespace DogHouse.ToonWorld.Map
{
    /// <summary>
    /// MapLocationVisualController is a script that 
    /// controls the visual aspect of a node on
    /// the map.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class MapLocationVisualController : MonoBehaviour
    {
        #region Public Variables
        [HideInInspector]
        public Action OnClicked;
        #endregion

        #region Private Variables
        [SerializeField]
        private Color m_defaultColor;

        [SerializeField]
        private Color m_highlightedColor;

        [SerializeField]
        private GameObject m_lineRendererPrefab;

        [SerializeField]
        private SpriteRenderer m_iconRenderer;

        [SerializeField]
        private float m_lineEarlyStop;

        [SerializeField]
        [Range(0.0001f, 1f)]
        private float m_screenSelectionRange;

        private ServiceReference<ICameraFinder> m_cameraFinder 
            = new ServiceReference<ICameraFinder>();

        private Animator m_animator;
        private Vector3 LineRendererOffset = new Vector3(0f, 0f, 0.0f);
        private MapIconState m_state = MapIconState.IDLE;
        #endregion

        #region Main Methods
        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        public void SetIcon(Sprite sprite)
        {
            m_iconRenderer.sprite = sprite;
        }

        public void SetOutput(GameObject output)
        {
            GameObject LineRendererObject = Instantiate(m_lineRendererPrefab);
            LineRendererObject.transform.SetParent(this.transform);
            LineRenderer renderer = LineRendererObject.GetComponent<LineRenderer>();

            Vector3[] positions = new Vector3[2];
            positions[0] = this.transform.position + LineRendererOffset;
            positions[1] = output.transform.position + LineRendererOffset;

            Vector3 fromTo = positions[1] - positions[0];
            fromTo.Normalize();
            positions[0] += fromTo * m_lineEarlyStop;
            positions[1] -= fromTo * m_lineEarlyStop;

            renderer.positionCount = 2;
            renderer.SetPositions(positions);
        }

        public void SetIconActive(bool value)
        {
            if (m_animator == null) m_animator.GetComponent<Animator>();
            m_animator?.SetBool("Available", value);
            m_state = (value) ? MapIconState.AVAILABLE : MapIconState.IDLE;
        }

        public void SetFull(bool value)
        {
            Color c = m_iconRenderer.color;
            c.a = (value) ? 1f : 0.5f;
            m_iconRenderer.color = c;
        }

        private void Update()
        {
            if (m_state != MapIconState.AVAILABLE) return;
            if (!m_cameraFinder.CheckServiceRegistered()) return;
            if (m_cameraFinder.Reference.Camera == null) return;

            Vector3 screenPosition = 
                m_cameraFinder.Reference.
                Camera.WorldToScreenPoint(this.transform.position);

            Vector3 mousePos = Input.mousePosition;

            screenPosition.z = 0f;
            mousePos.z = 0f;

            Vector3 fromTo = mousePos - screenPosition;
            fromTo.x = fromTo.x / Screen.width;
            fromTo.y = fromTo.y / Screen.height;

            if(fromTo.magnitude < m_screenSelectionRange)
            {
                m_iconRenderer.color = m_highlightedColor;
                return;
            }

            m_iconRenderer.color = m_defaultColor;
        }

        
        #endregion
    }

    public enum MapIconState
    {
        IDLE,
        AVAILABLE
    }
}
