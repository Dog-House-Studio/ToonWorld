using System.Collections.Generic;
using UnityEngine;

namespace DogHouse.ToonWorld.CombatControllers
{
    /// <summary>
    /// A Battlefield Unit is a unit on the battlefield. It has stats,
    /// a movement pattern and some form of data associated with the
    /// unit on the battlefield.
    /// </summary>
    public class BattleFieldUnit : GridObject
    {
        #region Public Variables
        public UnitClassType ClassType { get { return m_classDefinition; } set { m_classDefinition = value; } }
        #endregion

        #region Private Variables
        [SerializeField]
        private UnitClassType m_classDefinition;
        #endregion

        #region Main Methods

        public virtual List<Vector2Int> MoveLocations(Vector2Int gridPoint)
        {
            return m_classDefinition.Movement.MoveLocations(gridPoint);
        }
        #endregion
    }
}
