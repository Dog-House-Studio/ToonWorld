using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DogHouse.ToonWorld.CombatControllers
{
    public class EnemyDefinitionContainer : MonoBehaviour
    {
        [Header("Units to fight")]
        public List<GameUnitDefinition> unitDefinitions;

        public GameUnitDefinition GetRandomDefinition()
        {
            GameUnitDefinition randomDefinition = unitDefinitions[Random.Range(0,unitDefinitions.Count)];
            return randomDefinition;
        }
    }
}

