using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace DogHouse.ToonWorld.CombatControllers
{
    /// <summary>
    /// SkillTreeController is a script that controls
    /// the behaviour of the skill tree. 
    /// </summary>
    public class SkillTreeController : MonoBehaviour, IUnitIdentifier
    {
        #region Public Variables
        public float FadeValue => m_group.alpha;
        #endregion

        #region Private Variables
        [SerializeField]
        private CanvasGroup m_group;

        [SerializeField]
        private Image m_classEmblem;

        [SerializeField]
        private TMP_Text m_nameText;

        [SerializeField]
        private TMP_Text m_classNameText;
        #endregion

        #region Main Methods
        public void SetDataDisplay(GameUnitDefinition definition)
        {
            m_classEmblem.overrideSprite = definition.BaseClassType.ClassSprite;
            m_nameText.text = definition.UnitName;
            m_classNameText.text = definition.BaseClassType.ClassName;
        }

        public void SetFadeValue(float value)
        {
            m_group.alpha = Mathf.Clamp01(value);
        }
        #endregion
    }
}
