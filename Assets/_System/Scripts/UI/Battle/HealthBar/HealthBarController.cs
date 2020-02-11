using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DogScaffold;
using DogHouse.CoreServices;
using UnityEngine.Animations;

namespace DogHouse.ToonWorld.CombatControllers
{
    /// <summary>
    /// HealthBarController is a script that controls
    /// the visual aspects of the health bar for a 
    /// unit on the battlefield.
    /// </summary>
    public class HealthBarController : MonoBehaviour, IUnitIdentifier
    {
        #region Private Variables
        [SerializeField]
        private Image m_classEmblemImage;

        [SerializeField]
        private TMP_Text m_nameText;

        [SerializeField]
        private TMP_Text m_classNameText;

        [SerializeField]
        private TMP_Text m_unitLevelText;

        [SerializeField]
        private TMP_Text m_healthText;

        [SerializeField]
        private LookAtConstraint m_lookAtConstraint;

        private ServiceReference<ICameraFinder> m_cameraService 
            = new ServiceReference<ICameraFinder>();
        #endregion

        #region Main Methods
        void Start()
        {
            m_cameraService.AddRegistrationHandle(OnCameraServiceRegistered);
        }

        public void SetDataDisplay(GameUnitDefinition definition)
        {
            m_classEmblemImage.overrideSprite = definition.BaseClassType.ClassSprite;
            m_classNameText.text = definition.BaseClassType.ClassName;
            m_nameText.text = definition.UnitName;
            m_unitLevelText.text = definition.Level.ToString();
            SetHealthText(definition);
        }
        #endregion

        #region Utility Methods
        void OnCameraServiceRegistered()
        {
            ConstraintSource source = new ConstraintSource();
            source.weight = 1f;
            source.sourceTransform = m_cameraService.Reference.Camera.transform;
            m_lookAtConstraint.AddSource(source);
        }

        private void SetHealthText(GameUnitDefinition definition)
        {
            int maxHealth = definition.BaseClassType.BaseStats.Stats.MyDestroyableStats.Health.Value;
            string healthTextValue = "HP " + maxHealth.ToString() + " / " + maxHealth.ToString();
            m_healthText.text = healthTextValue;
        }
        #endregion
    }
}
