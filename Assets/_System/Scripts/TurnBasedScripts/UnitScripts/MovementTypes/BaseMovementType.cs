using System.Collections.Generic;
using UnityEngine;

namespace DogHouse.ToonWorld.CombatControllers
{
    /// <summary>
    /// BaseMovementType is a script that describes the
    /// movement type of a particular unit.
    /// </summary>
    public abstract class BaseMovementType : ScriptableObject
    {
        #region Protected Variables
        [SerializeField]
        protected int m_movementAmount;
        #endregion

        #region Main Methods
        public abstract List<Vector2Int> MoveLocations(Vector2Int gridPoint);
        #endregion
    }
}
