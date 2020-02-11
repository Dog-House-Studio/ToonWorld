using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DogScaffold;
using DogHouse.CoreServices;
using UnityEngine.Animations;
using static UnityEngine.Mathf;

namespace DogHouse.ToonWorld.CombatControllers
{
    /// <summary>
    /// HealthBarController is a script that controls
    /// the visual aspects of the health bar for a 
    /// unit on the battlefield.
    /// </summary>
    public class HealthBarController : MonoBehaviour, IUnitIdentifier
    {
        #region Public Variables
        public float FadeValue => m_canvasGroup.alpha;
        #endregion

        #region Private Variables
        [SerializeField]
        private CanvasGroup m_canvasGroup;

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

        public void SetFadeValue(float value)
        {
            m_canvasGroup.alpha = Clamp01(value);
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
            int maxHealth = definition.Stats.Health;
            string healthTextValue = "HP " + maxHealth.ToString() + " / " + maxHealth.ToString();
            m_healthText.text = healthTextValue;
        }
        #endregion
    }
}
