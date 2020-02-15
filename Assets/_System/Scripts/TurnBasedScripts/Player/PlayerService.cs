using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DogScaffold;

namespace DogHouse.ToonWorld.CombatControllers
{
    public class PlayerService : BaseService<IPlayerService>,IPlayerService
    {
        private List<GameUnitDefinition> unitList = new List<GameUnitDefinition>();

        public void AddUnit(GameUnitDefinition definition)
        {
            unitList.Add(definition);
        }

        public GameUnitDefinition GetUnitDefinition(int index)
        {
            if(index>unitList.Count || index < 0)
            {
                return null;
            }
            return unitList[index];
        }

        public int GetUnitCount()
        {
            return unitList.Count;
        }
    }
}

