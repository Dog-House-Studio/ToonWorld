﻿using UnityEngine;
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
        #region Public Variables
        public float FadeValue => m_UICanvasGroup.alpha;
        #endregion

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

        [SerializeField]
        private SkillTreeController m_skillTreeController;

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

            m_definition.OnExperienceGained -= OnExperienceCalculated;
            m_definition.OnExperienceGained += OnExperienceCalculated;

            m_experienceBarController.OnAnimationFinished -= HandleExperienceBarAnimationFinished;
            m_experienceBarController.OnAnimationFinished += HandleExperienceBarAnimationFinished;
        }

        public void OnDestroy()
        {
            m_definition.OnExperienceGained -= OnExperienceCalculated;
            m_experienceBarController.OnAnimationFinished -= HandleExperienceBarAnimationFinished;
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

        public void DisplaySkillTree(bool value)
        {
            StartCoroutine(_FadeItem(m_skillTreeController, value));
        }

        public void AddExperience(int amount)
        {
            m_definition.AddExperience(amount);
        }

        private void OnExperienceCalculated(float percent)
        {
            DisplayHealthBar(false);
            StartCoroutine(_FadeItem(m_experienceBarController, true,
                () => { SetVisualExperienceBarValue(percent); }));
        }

        public void ApplyHealthChange(int delta)
        {
            m_definition.ChangeHealth(delta);
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

            bool skip = false;
            if (fadeIn && Approximately(1f, fader.FadeValue)) skip = true;
            if (!fadeIn && Approximately(0f, fader.FadeValue)) skip = true;

            if (!skip)
            {
                do
                {
                    timePassed += Time.deltaTime;
                    lerpValue = m_UIFadeCurve.Evaluate(Clamp01(timePassed / m_UIFadeTime));
                    value = (fadeIn) ? lerpValue : 1f - lerpValue;

                    fader.SetFadeValue(value);
                    yield return null;

                } while (timePassed < m_UIFadeTime);
            }

            yield return null;
            callback?.Invoke();
        }

        private void SetVisualExperienceBarValue(float value)
        {
            m_experienceBarController.SetValue(value);
        }

        private void HandleExperienceBarAnimationFinished()
        {
            DisplayExperienceBar(false);
            DisplayHealthBar(true);
        }
        #endregion
    }
}
