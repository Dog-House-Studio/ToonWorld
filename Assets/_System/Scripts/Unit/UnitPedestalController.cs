using UnityEngine;
using UnityEngine.UI;
using DogHouse.ToonWorld.CombatControllers;

namespace DogHouse.ToonWorld.Unit
{
    /// <summary>
    /// UnitPedestalController is a script that 
    /// controls the unit pedestal. The unit pedestal
    /// is responsible for controlling the visuals of
    /// the pedestal and knowing when the player has clicked
    /// on that particular pedestal.
    /// </summary>
    public class UnitPedestalController : MonoBehaviour, IUnitIdentifier
    {
        #region Public Variables
        public float FadeValue => m_group.alpha;
        #endregion

        #region Private Variables
        [SerializeField]
        private Image m_emblemImage;

        [SerializeField]
        private CanvasGroup m_group;
        #endregion

        #region Main Methods
        public void SetFadeValue(float value)
        {
            m_group.alpha = value;
        }

        public void SetDataDisplay(GameUnitDefinition definition)
        {
            m_emblemImage.overrideSprite = definition.BaseClassType.ClassSprite;
        }

        void Start()
        {
            m_emblemImage.color = Color.gray;
        }
        #endregion
    }
}
