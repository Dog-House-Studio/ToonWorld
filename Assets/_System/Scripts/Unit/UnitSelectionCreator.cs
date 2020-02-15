using UnityEngine;
using DogHouse.ToonWorld.CombatControllers;

namespace DogHouse.ToonWorld.Unit
{
    /// <summary>
    /// UnitSelectionController is a script that controls
    /// the creation of the player selection.
    /// </summary>
    public class UnitSelectionCreator : MonoBehaviour
    {
        #region Private Variables
        [SerializeField]
        private GameObject m_pedestalPrefab;

        [SerializeField]
        private int m_numberOfOptions;

        [SerializeField]
        private float m_spacing;
        #endregion

        #region Main Methods
        private void Start()
        {
            float offset = m_numberOfOptions * m_spacing;
            Vector3 position = this.transform.position;
            position.x -= offset * 0.5f;

            for(int i = 0; i < m_numberOfOptions; i++)
            {
                CreatePedestal(position);
                position.x += m_spacing;
            }
        }
        #endregion

        #region Utility Methods
        private void CreatePedestal(Vector3 pos)
        {
            GameObject pedestal = Instantiate(m_pedestalPrefab);
            pedestal.transform.position = pos;
            pedestal.transform.SetParent(transform);
        }
        #endregion
    }
}
