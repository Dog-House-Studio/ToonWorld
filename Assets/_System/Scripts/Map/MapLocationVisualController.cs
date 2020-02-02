using UnityEngine;

namespace DogHouse.ToonWorld.Map
{
    /// <summary>
    /// MapLocationVisualController is a script that 
    /// controls the visual aspect of a node on
    /// the map.
    /// </summary>
    public class MapLocationVisualController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField]
        private GameObject m_lineRendererPrefab;

        [SerializeField]
        private SpriteRenderer m_iconRenderer;

        [SerializeField]
        private float m_lineEarlyStop;

        private Vector3 LineRendererOffset = new Vector3(0f, 0f, 0.0f);
        #endregion

        #region Main Methods
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
        #endregion
    }
}
