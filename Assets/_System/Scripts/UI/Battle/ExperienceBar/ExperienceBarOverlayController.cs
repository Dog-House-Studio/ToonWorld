﻿using UnityEngine;
using System;

namespace DogHouse.ToonWorld.UI
{
    /// <summary>
    /// ExperienceBarOverlayController is a script that controls
    /// the overlay on the experience bar.
    /// </summary>
    public class ExperienceBarOverlayController : MonoBehaviour
    {
        #region Public Variables
        [HideInInspector]
        public Action OnValueChanged;
        #endregion

        #region Private Variables
        [SerializeField]
        private Animator m_animator;

        private const string TRIGGER_NAME = "Play";
        #endregion

        #region Main Methods
        public void Play()
        {
            m_animator?.SetTrigger(TRIGGER_NAME);
        }

        public void ChangeValue()
        {
            OnValueChanged?.Invoke();
        }
        #endregion
    }
}
