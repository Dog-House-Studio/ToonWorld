using UnityEngine;
using DogHouse.ToonWorld.Animation;
using System.Collections;
using System;
using static UnityEngine.Mathf;

namespace DogHouse.ToonWorld.CombatControllers
{
    /// <summary>
    /// UnitRootController is a script that controls
    /// the root of a unit on the battlefield. 
    /// 
    /// This should be able to setup and create all 
    /// visuals for the root object given a unit 
    /// definition file.
    /// </summary>
    public class UnitRootController : MonoBehaviour, IFade
    {
        #region Private Variables
        [Header("Elements")]
        [SerializeField]
        private GameObject m_modelParent;

        [SerializeField]
        private CanvasGroup m_UICanvasGroup;

        [SerializeField]
        private HealthBarController m_healthBarController;

        [SerializeField]
        private ExperienceBarController m_experienceBarController;

        [Header("Transitions")]
        [SerializeField]
        [Range(0f, 10f)]
        private float m_UIFadeTime;

        [SerializeField]
        private AnimationCurve m_UIFadeCurve;

        private GameUnitDefinition m_definition;
        #endregion

        #region Main Methods
        public void CreateUnit(GameUnitDefinition definition)
        {
            m_definition = definition;
            CreateUnitModel(definition);
            SetUnitIdentifiers(definition);
            SetFadeValue(1f);
        }

        public void SetFadeValue(float value)
        {
            m_UICanvasGroup.alpha = Clamp01(value);
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            CancelInvoke();
        }

        public void DisplayUnitIdentifiers(bool value)
        {
            StartCoroutine(_FadeItem(this, value));
        }

        public void DisplayHealthBar(bool value)
        {
            StartCoroutine(_FadeItem(m_healthBarController, value));
        }

        public void DisplayExperienceBar(bool value)
        {
            StartCoroutine(_FadeItem(m_experienceBarController, value));
        }

        public void AddExperience(int amount)
        {
            m_definition.AddExperience(amount);
        }
        #endregion

        #region Utility Methods
        private void CreateUnitModel(GameUnitDefinition definition)
        {
            GameObject model = Instantiate(definition.Model);
            model.transform.SetParent(m_modelParent.transform);
            model.transform.localPosition = Vector3.zero;
        }

        private void SetUnitIdentifiers(GameUnitDefinition definition)
        {
            IUnitIdentifier[] identifiers = GetComponentsInChildren<IUnitIdentifier>();
            foreach(IUnitIdentifier identifier in identifiers)
            {
                identifier?.SetDataDisplay(definition);
                identifier?.SetFadeValue(0f);
            }
        }

        private IEnumerator _FadeItem(IFade fader, bool fadeIn, Action callback = null)
        {
            float timePassed = 0f;
            float lerpValue = 0f;
            float value = 0;

            do
            {
                timePassed += Time.deltaTime;
                lerpValue = m_UIFadeCurve.Evaluate(Clamp01(timePassed / m_UIFadeTime));
                value = (fadeIn) ? lerpValue : 1f - lerpValue;

                fader.SetFadeValue(value);
                yield return null;
                
            } while (timePassed < m_UIFadeTime);

            yield return null;
            callback?.Invoke();
        }
        #endregion
    }
}
