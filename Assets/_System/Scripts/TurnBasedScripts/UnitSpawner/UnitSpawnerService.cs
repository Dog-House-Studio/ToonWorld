using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DogScaffold;

namespace DogHouse.ToonWorld.CombatControllers
{
    public class UnitSpawnerService : BaseService<IUnitSpawnerService>,IUnitSpawnerService
    {
        [SerializeField]
        private GameObject m_unitRootPrefab;

        public GameObject SpawnUnit(GameUnitDefinition definition, bool player)
        {
            GameObject unit = Instantiate(m_unitRootPrefab);
            UnitRootController controller = unit.GetComponent<UnitRootController>();
            if (!player)
            {
                GameUnitDefinition clone = Instantiate(definition);
                controller.CreateUnit(clone);
            }
            else
            {
                controller.CreateUnit(definition);
            }
            
            return unit;
        }
    }
}

