using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [Header("Selected Unit")]
    public GridObject selectedUnit = null;   // Currently highlighted GridTile

    [Header("Players")]
    public CombatPlayer gamePlayer = new CombatPlayer("Player");
    public CombatPlayer enemyPlayer = new CombatPlayer("Enemy");

    public CombatPlayer currentPlayer;
    public CombatPlayer otherPlayer;

    public static CombatManager Instance = null;

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
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
        var piececomp = unit as BaseUnitClass;
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
