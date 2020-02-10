using UnityEngine;

namespace DogHouse.ToonWorld.CombatControllers
{
    /// <summary>
    /// GameUnitDefinition is a script that holds the
    /// data for any particular game unit. This definition
    /// serializes all the data of a particular unit so
    /// that it may be created at runtime on the battlefield.
    /// </summary>
    [CreateAssetMenu(menuName = "Dog House/ToonWorld/Unit/Game Unit Definition", fileName = "MyNewGameUnitDefinition")]
    public class GameUnitDefinition : ScriptableObject
    {
        #region Public Variables
        public GameObject Model => m_model;
        #endregion

        #region Private Variables
        [SerializeField]
        private UnitClassType m_baseType;

        [SerializeField]
        private string m_unitName;

        [SerializeField]
        private GameObject m_model;

        [SerializeField]
        private int m_level;
        #endregion

        #region Main Methods

        #endregion
    }
}
