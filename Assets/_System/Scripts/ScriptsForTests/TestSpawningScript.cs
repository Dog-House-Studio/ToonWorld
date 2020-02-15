using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DogScaffold;

namespace DogHouse.ToonWorld.CombatControllers
{
    public class TestSpawningScript : MonoBehaviour
    {
        public List<GameUnitDefinition> playerUnitDefinitions;
        private ServiceReference<ICombatManager> CombatManager = new ServiceReference<ICombatManager>();
        private ServiceReference<IPlayerService> PlayerManager = new ServiceReference<IPlayerService>();

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (!CombatManager.CheckServiceRegistered())
                {
                    return;
                }
                CombatManager.Reference.InitializeUnits();
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (!PlayerManager.CheckServiceRegistered())
                {
                    return;
                }
                if (playerUnitDefinitions.Count == 0)
                {
                    Debug.Log("Empty Array");
                    return;
                }
                PlayerManager.Reference.AddUnit(GetRandomDefinition());
            }
        }

        private GameUnitDefinition GetRandomDefinition()
        {
            GameUnitDefinition random = playerUnitDefinitions[Random.Range(0, playerUnitDefinitions.Count)];
            return random;
        }
    }
}

