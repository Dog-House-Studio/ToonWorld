using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType { Swordsman}

public abstract class BaseUnitClass : GridObject
{

    [Header("Unit Type")]
    public UnitType type;

    protected Vector2Int[] dir = {new Vector2Int(0,1), new Vector2Int(1, 0), new Vector2Int(0, -1), new Vector2Int(-1, 0)};

    public abstract List<Vector2Int> MoveLocations(Vector2Int gridPoint);
}
