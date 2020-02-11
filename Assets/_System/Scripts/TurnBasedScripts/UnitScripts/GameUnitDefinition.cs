﻿using UnityEngine;
using System;

namespace DogHouse.ToonWorld.CombatControllers
{
    /// <summary>
    /// GameUnitDefinition is a script that holds the
    /// data for any particular game unit. This definition
    /// serializes all the data of a particular unit so
    /// that it may be created at runtime on the battlefield.
    /// </summary>
    [CreateAssetMenu(menuName = "Dog House/ToonWorld/Unit/Game Unit Definition", fileName = "MyNewGameUnitDefinition")]
    public class GameUnitDefinition : ScriptableObject
    {
        #region Public Variables
        [HideInInspector]
        public Action<float> OnExperienceGained;

        public GameObject Model => m_model;
        public UnitClassType BaseClassType => m_baseType;
        public string UnitName => m_unitName;
        public int Level => m_level;
        public UnitStats Stats => m_stats;
        #endregion

        #region Private Variables
        [SerializeField]
        private UnitClassType m_baseType;

        [SerializeField]
        private string m_unitName;

        [SerializeField]
        private GameObject m_model;

        [SerializeField]
        private int m_level;

        private UnitStats m_stats;
        private int m_experience = 0;
        private int m_levelExperienceTarget = 0;
        #endregion

        #region Main Methods
        //We will need to change this to a separate method. A unit service
        //will need to be written so that all the different types of
        //GameUnitDefinitions we've designed aren't created all at once.
        private void OnEnable()
        {
            m_stats = m_baseType.BaseStats;
            m_levelExperienceTarget = m_baseType.CalculateExperienceNeeded(m_level + 1);
        }

        public void AddExperience(int amount)
        {
            m_experience += amount;
            if (m_experience > m_levelExperienceTarget) m_experience = m_levelExperienceTarget;

            float percentage = ((float)m_experience) / ((float)m_levelExperienceTarget);
            OnExperienceGained?.Invoke(percentage);

            if(m_experience == m_levelExperienceTarget)
            {
                m_level++;
                m_experience = 0;
                m_levelExperienceTarget = m_baseType.CalculateExperienceNeeded(m_level + 1);
            }
        }
        #endregion
    }
}
