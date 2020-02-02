using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CombatPlayer
{
    public List<GridObject> units;

    public string name;

    public CombatPlayer(string name)
    {
        this.name = name;

        units = new List<GridObject>();
    }
}
