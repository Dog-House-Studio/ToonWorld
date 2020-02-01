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
        private LineRenderer m_lineRenderer;

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
            Vector3[] positions = new Vector3[2];
            positions[0] = this.transform.position;
            positions[1] = output.transform.position;
        }
        #endregion
    }
}
