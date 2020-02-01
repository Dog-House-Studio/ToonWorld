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
            positions[0] = this.transform.position;
            positions[1] = output.transform.position;

            renderer.positionCount = 2;
            renderer.SetPositions(positions);
        }
        #endregion
    }
}
