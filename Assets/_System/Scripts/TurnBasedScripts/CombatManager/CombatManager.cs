using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DogScaffold;

namespace DogHouse.ToonWorld.CombatControllers
{
    public class CombatManager : BaseService<ICombatManager>,ICombatManager
    {
        [SerializeField]
        private GridObject SelectedUnit = null;   // Currently highlighted GridTile
        [SerializeField]
        private CombatPlayer GamePlayer = new CombatPlayer("Player");
        [SerializeField]
        private CombatPlayer EnemyPlayer = new CombatPlayer("Enemy");

        private CombatPlayer currentPlayer;
        private CombatPlayer otherPlayer;

        public GridObject selectedUnit { get { return SelectedUnit; } set { SelectedUnit = value; } }
        public CombatPlayer gamePlayer { get { return GamePlayer; } set { GamePlayer = value; } }
        public CombatPlayer enemyPlayer { get { return EnemyPlayer; } set { EnemyPlayer = value; } }

        private ServiceReference<IPlayerService> PlayerService = new ServiceReference<IPlayerService>();
        private ServiceReference<IUnitSpawnerService> UnitSpawnService = new ServiceReference<IUnitSpawnerService>();

        // Start is called before the first frame update
        public void InitializeUnits()
        {
            SpawnAllUnits();

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
        private void SpawnAllUnits()
        {
            SpawnPlayerUnits();
            SpawnEnemyUnits();
   
        }
        private void SpawnPlayerUnits()
        {
            int playerUnitCount = PlayerService.Reference.GetUnitCount();
            for (int i = 0; i < playerUnitCount; i++)
            {
                GridObject temp = UnitSpawnService.Reference.SpawnUnit(PlayerService.Reference.GetUnitDefinition(i), true).GetComponent<GridObject>();
                gamePlayer.units.Add(temp);
            }
            GameObject[] playerSpawnPoints = GameObject.FindGameObjectsWithTag("PlayerSpawnPoint");
            for(int a = 0; a < playerUnitCount; a++)
            {
                gamePlayer.units[a].transform.position = playerSpawnPoints[a].transform.position;
                gamePlayer.units[a].SetCurrentGridTile(playerSpawnPoints[a].GetComponent<GridTile>());
            }
        }
        private void SpawnEnemyUnits()
        {
            EnemyDefinitionContainer enemyDefinitionContainer = FindObjectOfType<EnemyDefinitionContainer>();
            GameObject[] enemySpawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawnPoint");
            int enemyCounter = enemySpawnPoints.Length;
            for(int i = 0; i < enemyCounter; i++)
            {
                enemyPlayer.units.Add(UnitSpawnService.Reference.SpawnUnit(enemyDefinitionContainer.GetRandomDefinition(), false).GetComponent<GridObject>());
                enemyPlayer.units[i].transform.position = enemySpawnPoints[i].transform.position;
                enemyPlayer.units[i].SetCurrentGridTile(enemySpawnPoints[i].GetComponent<GridTile>());
            }

        }
    }
}

