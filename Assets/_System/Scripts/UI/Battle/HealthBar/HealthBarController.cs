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
        private Slider m_healthSlider;

        [SerializeField]
        private LookAtConstraint m_lookAtConstraint;

        private GameUnitDefinition m_definition;

        private ServiceReference<ICameraFinder> m_cameraService 
            = new ServiceReference<ICameraFinder>();
        #endregion

        #region Main Methods
        void Start()
        {
            m_cameraService.AddRegistrationHandle(OnCameraServiceRegistered);
        }

        void OnDestroy()
        {
            if(m_definition != null)
            {
                m_definition.OnStatsChanged -= HandleStatChange;
            }
        }

        public void SetDataDisplay(GameUnitDefinition definition)
        {
            m_classEmblemImage.overrideSprite = definition.BaseClassType.ClassSprite;
            m_classNameText.text = definition.BaseClassType.ClassName;
            m_nameText.text = definition.UnitName;
            m_unitLevelText.text = definition.Level.ToString();
            SetHealthText(definition.Stats, definition.BaseStats);

            m_definition = definition;
            m_definition.OnStatsChanged -= HandleStatChange;
            m_definition.OnStatsChanged += HandleStatChange;
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

        private void SetHealthText(UnitStats currentStats, UnitStats baseStats)
        {
            string healthTextValue = "HP " + currentStats.Health.ToString() 
                + " / " + baseStats.Health.ToString();

            m_healthText.text = healthTextValue;

            m_healthSlider.value = ((float)currentStats.Health) / ((float)baseStats.Health);
        }

        private void HandleStatChange(UnitStats currentStats, UnitStats baseStats)
        {
            SetHealthText(currentStats, baseStats);
        }
        #endregion
    }
}
