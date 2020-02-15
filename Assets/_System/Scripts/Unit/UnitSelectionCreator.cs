using UnityEngine;
using DogHouse.ToonWorld.CombatControllers;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

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
        private GameObject m_playerRootPrefab;

        [SerializeField]
        private GameUnitDefinition[] m_definitions;

        [SerializeField]
        private float m_spacing;

        private List<UnitPedestalController> m_pedestalControllers 
            = new List<UnitPedestalController>();
        #endregion

        #region Main Methods
        private void Start()
        {
            Vector3 position = transform.position;
            position.x -= m_definitions.Length * m_spacing * 0.5f;

            for(int i = 0; i < m_definitions.Length; i++)
            {
                CreatePedestal(position, m_definitions[i]);
                position.x += m_spacing;
            }
        }

        private void OnPedestalBecameActive(UnitPedestalController controller)
        {
            for(int i = 0; i < m_pedestalControllers.Count; i++)
            {
                if (m_pedestalControllers[i] == controller)
                {
                    m_pedestalControllers[i].SetState(PedestalState.ACTIVE);
                    continue;
                }
                m_pedestalControllers[i].SetState(PedestalState.DISABLED);
            }
        }
        #endregion

        #region Utility Methods
        private void CreatePedestal(Vector3 pos, GameUnitDefinition definition)
        {
            GameObject pedestal = Instantiate(m_pedestalPrefab);
            pedestal.transform.position = pos;
            pedestal.transform.SetParent(transform);

            GameObject root = Instantiate(m_playerRootPrefab);
            root.transform.SetParent(pedestal.transform);
            root.transform.localPosition = Vector3.up;

            UnitRootController controller = root.GetComponent<UnitRootController>();
            controller?.CreateUnit(definition);

            UnitPedestalController identifier = pedestal.GetComponent<UnitPedestalController>();
            identifier.SetDataDisplay(definition);

            m_pedestalControllers.Add(identifier);

            identifier.OnSetActive -= OnPedestalBecameActive;
            identifier.OnSetActive += OnPedestalBecameActive;
        }
        #endregion
    }
}
