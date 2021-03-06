﻿using UnityEngine;

namespace DogHouse.ToonWorld.CombatControllers
{
    /// <summary>
    /// A Unit class type is a scriptable object
    /// that contains the definition for a 
    /// unit class type.
    /// </summary>
    [CreateAssetMenu(menuName ="Dog House/ToonWorld/Unit/Unit Class", fileName ="MyNewUnitClass")]
    public class UnitClassType : ScriptableObject
    {
        #region Public Variables
        public string ClassName => m_className;

        public Sprite ClassSprite => m_classSprite;

        public UnitClassType[] Weakness => m_weakness;
        public UnitClassType[] Strength => m_strength;
        public BaseMovementType Movement => m_movementType;
        public ExperienceType Experience => m_experienceType;
        public UnitStats BaseStats => m_baseStats;
        #endregion

        #region Private Variables
        [SerializeField]
        private string m_className;
        
        [SerializeField]
        private Sprite m_classSprite;

        [SerializeField]
        private BaseMovementType m_movementType;

        [SerializeField]
        private UnitStats m_baseStats;

        [SerializeField]
        private ExperienceType m_experienceType;

        [SerializeField]
        private UnitClassType[] m_weakness;

        [SerializeField]
        private UnitClassType[] m_strength;
        #endregion

        #region Main Methods
        public int CalculateExperienceNeeded(int level)
        {
            float multiplier = 1f;
            if (m_experienceType == ExperienceType.FAST) multiplier = 0.75f;
            if (m_experienceType == ExperienceType.SLOW) multiplier = 1.25f;

            int value = (int)((float)(level * 8) * multiplier);
            value = Mathf.Max(1, value);

            return value;
        }
        #endregion
    }

    public enum ExperienceType
    {
        SLOW,
        REGULAR,
        FAST
    }
}
