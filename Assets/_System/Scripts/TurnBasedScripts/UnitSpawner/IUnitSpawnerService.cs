using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DogScaffold;

namespace DogHouse.ToonWorld.CombatControllers
{
    public interface IUnitSpawnerService : IService
    {
        GameObject SpawnUnit(GameUnitDefinition definition, bool player);
    }
}

