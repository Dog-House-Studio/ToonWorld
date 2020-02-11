using UnityEngine;

namespace DogHouse.ToonWorld.CombatControllers
{
    [System.Serializable]
    public class DestroyableStats
    {
        public Stat Health = new Stat("Health", "HP", 0);
        public Stat Defence = new Stat("Defence", "DEF", 0);
    }

    [CreateAssetMenu(menuName = "Dog House/ToonWorld/Unit/Base Stats", fileName = "MyNewClassStats")]
    public class UnitStats : ScriptableObject
    {
        #region Public Variables
        public int Health => m_myDestroyableStats.Health.Value;
        public int Defence => m_myDestroyableStats.Defence.Value;
        public int Strength => m_strength.Value;
        public int Accuracy => m_accuracy.Value;
        public int Speed => m_speed.Value;
        public int Luck => m_luck.Value;
        #endregion

        #region Private Variables
        [SerializeField]
        private DestroyableStats m_myDestroyableStats;

        [SerializeField]
        private Stat m_strength = new Stat("Strength", "STR", 0);

        [SerializeField]
        private Stat m_accuracy = new Stat("Accuracy", "ACC", 0);

        [SerializeField]
        private Stat m_speed = new Stat("Speed", "SPD", 0);

        [SerializeField]
        private Stat m_luck = new Stat("Luck", "LCK", 0);
        #endregion
    }

    [System.Serializable]
    public struct Stat
    {
        #region Public Variables
        public int Value;
        #endregion

        #region Private Variables
        private string StatName;
        private string StatShortHandName;
        #endregion

        public Stat(string name, string shortHandName, int value)
        {
            StatName = name;
            StatShortHandName = shortHandName;
            Value = value;
        }
    }
}
