using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DogHouse.ToonWorld.CombatControllers
{
    /// <summary>
    /// A Unit class type is a scriptable object
    /// that contains the definition for a 
    /// unit class type.
    /// </summary>
    [CreateAssetMenu(menuName ="Dog House/ToonWorld/Unit Class", fileName ="MyNewUnitClass")]
    public class UnitClassType : ScriptableObject
    {
        #region Public Variables
        public string ClassName => m_className;

        public Sprite ClassSprite => m_classSprite;

        public UnitClassType[] Weakness => m_weakness;
        public UnitClassType[] Strength => m_strength;
        #endregion

        #region Private Variables
        [SerializeField]
        private string m_className;

        [SerializeField]
        private Sprite m_classSprite;

        [SerializeField]
        private UnitClassType[] m_weakness;

        [SerializeField]
        private UnitClassType[] m_strength;
        #endregion
    }
}
