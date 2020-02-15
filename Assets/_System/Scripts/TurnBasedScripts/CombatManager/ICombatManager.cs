using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DogScaffold;

namespace DogHouse.ToonWorld.CombatControllers
{
    public interface ICombatManager : IService
    {
        GridObject selectedUnit { get; set; }
        CombatPlayer gamePlayer { get; set; }
        CombatPlayer enemyPlayer { get; set; }

        void InitializeUnits();

        GridObject TrySelectUnitAtTile(GridTile targetTile);

        void DeselectUnit();

        bool DoesUnitBelongToCurrentPlayer(GridObject unit);

        List<Vector2Int> MovesForUnit(GridObject unit);

        bool FriendlyUnitAt(Vector2Int gridPosition);

        void NextPlayer();
    }
}

