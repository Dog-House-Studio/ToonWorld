using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DogScaffold;

namespace DogHouse.ToonWorld.CombatControllers
{
    public class CombatManager : BaseService<ICombatManager>,ICombatManager
    {
        private GridObject SelectedUnit = null;   // Currently highlighted GridTile

        private CombatPlayer GamePlayer = new CombatPlayer("Player");
        private CombatPlayer EnemyPlayer = new CombatPlayer("Enemy");

        private CombatPlayer currentPlayer;
        private CombatPlayer otherPlayer;

        public GridObject selectedUnit { get { return SelectedUnit; } set { SelectedUnit = value; } }
        public CombatPlayer gamePlayer { get { return GamePlayer; } set { GamePlayer = value; } }
        public CombatPlayer enemyPlayer { get { return EnemyPlayer; } set { EnemyPlayer = value; } }

        // Start is called before the first frame update
        void Start()
        {
            currentPlayer = gamePlayer;
            otherPlayer = enemyPlayer;
        }

        public GridObject TrySelectUnitAtTile(GridTile targetTile)
        {
            var unit = GridManager.GetGridObjectAtPosition(targetTile.m_GridPosition);

            if (DoesUnitBelongToCurrentPlayer(unit))
            {
                selectedUnit = unit;
                return unit;
            }

            return null;
        }

        public void DeselectUnit()
        {
            selectedUnit = null;
        }

        public bool DoesUnitBelongToCurrentPlayer(GridObject unit)
        {
            return currentPlayer.units.Contains(unit);
        }

        public List<Vector2Int> MovesForUnit(GridObject unit)
        {
            var piececomp = unit as BattleFieldUnit;
            var locations = piececomp.MoveLocations(unit.m_GridPosition);

            // filter out locations with friendly piece
            locations.RemoveAll(tile => FriendlyUnitAt(tile));

            return locations;
        }

        public bool FriendlyUnitAt(Vector2Int gridPosition)
        {
            GridObject unit = GridManager.GetGridObjectAtPosition(gridPosition);

            if (unit == null)
            {
                return false;
            }

            if (otherPlayer.units.Contains(unit))
            {
                return false;
            }

            return true;
        }
        public void NextPlayer()
        {
            var tempPlayer = currentPlayer;
            currentPlayer = otherPlayer;
            otherPlayer = tempPlayer;

        }
    }
}

