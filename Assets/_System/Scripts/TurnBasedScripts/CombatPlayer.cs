using System;
using System.Collections.Generic;
using UnityEngine;

namespace DogHouse.ToonWorld.CombatControllers
{
    //This class is used to distinguise between the player and the enemy ai.
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
}

