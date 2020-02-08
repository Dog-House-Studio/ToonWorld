using UnityEngine;

namespace DogHouse.ToonWorld.CombatControllers
{
    [System.Serializable]
    public class DestroyableStats
    {
        public Stat Health = new Stat("Health", "HP", 0);
        public Stat Defence = new Stat("Defence", "DEF", 0);
    }

    [System.Serializable]
    public class BattlefieldUnitStats
    {
        public DestroyableStats MyDestroyableStats;
        public Stat Strength = new Stat("Strength", "STR", 0);
        public Stat Accuracy = new Stat("Accuracy", "ACC", 0);
        public Stat Speed = new Stat("Speed", "SPD", 0);
        public Stat Luck = new Stat("Luck", "LCK", 0);
    }

    [CreateAssetMenu(menuName = "Dog House/ToonWorld/Unit/Base Stats", fileName = "MyNewClassStats")]
    public class BaseClassUnitStats : ScriptableObject
    {
        public BattlefieldUnitStats Stats;
    }

    [System.Serializable]
    public struct Stat
    {
        public string StatName;
        public string StatShortHandName;
        public int Value;

        public Stat(string name, string shortHandName, int value)
        {
            StatName = name;
            StatShortHandName = shortHandName;
            Value = value;
        }
    }
}
