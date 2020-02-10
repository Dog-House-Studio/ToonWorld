using UnityEngine;

namespace DogHouse.ToonWorld.CombatControllers
{
    /// <summary>
    /// UnitRootController is a script that controls
    /// the root of a unit on the battlefield. 
    /// 
    /// This should be able to setup and create all 
    /// visuals for the root object given a unit 
    /// definition file.
    /// </summary>
    public class UnitRootController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField]
        private GameObject m_modelParent;

        private GameUnitDefinition m_definition;
        #endregion

        #region Main Methods
        public void CreateUnit(GameUnitDefinition definition)
        {
            m_definition = definition;
            CreateUnitModel(definition);
        }
        #endregion

        #region Utility Methods
        private void CreateUnitModel(GameUnitDefinition definition)
        {
            GameObject model = Instantiate(definition.Model);
            model.transform.SetParent(m_modelParent.transform);
            model.transform.localPosition = Vector3.zero;
        }
        #endregion
    }
}
