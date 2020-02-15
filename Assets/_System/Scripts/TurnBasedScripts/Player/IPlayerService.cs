using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DogScaffold;

namespace DogHouse.ToonWorld.CombatControllers
{
    public interface IPlayerService : IService
    {
        void AddUnit(GameUnitDefinition definition);
        GameUnitDefinition GetUnitDefinition(int index);
        int GetUnitCount();
    }
}

