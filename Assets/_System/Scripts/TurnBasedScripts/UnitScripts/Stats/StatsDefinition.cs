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
        public int Health
        {
            get { return m_myDestroyableStats.Health.Value; }
            set { m_myDestroyableStats.Health.Value = value; }
        }

        public int Defence => m_myDestroyableStats.Defence.Value;
        public int Strength => m_strength.Value;
        public int Accuracy => m_accuracy.Value;
        public int Speed => m_speed.Value;
        public int Luck => m_luck.Value;
        #endregion

        #region Private Variables
        [SerializeField]
        private DestroyableStats m_myDestroyableStats = new DestroyableStats();

        [SerializeField]
        private Stat m_strength = new Stat("Strength", "STR", 0);

        [SerializeField]
        private Stat m_accuracy = new Stat("Accuracy", "ACC", 0);

        [SerializeField]
        private Stat m_speed = new Stat("Speed", "SPD", 0);

        [SerializeField]
        private Stat m_luck = new Stat("Luck", "LCK", 0);
        #endregion

        #region Main Methods
        public void Copy(UnitStats stats)
        {
            m_myDestroyableStats.Health.Value = stats.Health;
            m_myDestroyableStats.Defence.Value = stats.Defence;
            m_strength.Value = stats.Strength;
            m_accuracy.Value = stats.Accuracy;
            m_speed.Value = stats.Speed;
            m_luck.Value = stats.Luck;
        }
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
