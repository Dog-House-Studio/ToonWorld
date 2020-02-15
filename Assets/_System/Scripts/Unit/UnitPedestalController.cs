﻿using UnityEngine;
using UnityEngine.UI;
using DogHouse.ToonWorld.CombatControllers;
using DogScaffold;
using DogHouse.CoreServices;
using System;
using TMPro;

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
        public Action<UnitPedestalController> OnSetActive;
        public float FadeValue => m_group.alpha;
        #endregion

        #region Private Variables
        [Header("Elements")]
        [SerializeField]
        private Image m_emblemImage;

        [SerializeField]
        private CanvasGroup m_group;

        [SerializeField]
        private Light m_lightObject;

        [SerializeField]
        private TMP_Text m_nameText;

        [SerializeField]
        private GameObject m_unitInformationParent;

        [Header("Settings")]
        [SerializeField]
        [Range(0f, 1f)]
        private float m_highlightRange;

        private ServiceReference<ICameraFinder> m_cameraFinderService 
            = new ServiceReference<ICameraFinder>();

        private PedestalState m_state = PedestalState.DISABLED;
        #endregion

        #region Main Methods
        public void SetFadeValue(float value)
        {
            m_group.alpha = value;
        }

        public void SetDataDisplay(GameUnitDefinition definition)
        {
            m_emblemImage.overrideSprite = definition.BaseClassType.ClassSprite;
            m_nameText.text = definition.UnitName;
        }

        void Start()
        {
            SetState(PedestalState.IDLE);
        }

        void Update()
        {
            if (m_state != PedestalState.IDLE && m_state != PedestalState.SELECTED) return;
            if (!m_cameraFinderService.CheckServiceRegistered()) return;

            Vector3 mousePos = Input.mousePosition;
            float xPos = mousePos.x / (float)Screen.width;
            Vector3 screenPos = m_cameraFinderService.Reference.Camera.WorldToScreenPoint(transform.position);
            if(Mathf.Abs(xPos - screenPos.x / (float)Screen.width) < m_highlightRange)
            {
                SetState(PedestalState.SELECTED);
                if (Input.GetMouseButtonUp(0)) OnSetActive?.Invoke(this);
                return;
            }

            SetState(PedestalState.IDLE);
        }
        #endregion

        #region Utility Methods
        public void SetState(PedestalState newState)
        {
            if (m_state == newState) return;

            m_state = newState;

            if (newState == PedestalState.IDLE)
            {
                m_emblemImage.color = Color.gray;
                m_lightObject.gameObject.SetActive(false);
                m_unitInformationParent.SetActive(false);
                return;
            }

            if(newState == PedestalState.SELECTED)
            {
                m_emblemImage.color = Color.white;
                m_lightObject.gameObject.SetActive(true);
                m_unitInformationParent.SetActive(false);
                return;
            }

            if (newState == PedestalState.ACTIVE)
            {
                m_emblemImage.color = Color.white;
                m_lightObject.gameObject.SetActive(true);
                m_unitInformationParent.SetActive(true);
                return;
            }

            if (newState == PedestalState.DISABLED)
            {
                m_emblemImage.color = Color.gray;
                m_lightObject.gameObject.SetActive(false);
                m_unitInformationParent.SetActive(false);
                return;
            }
        }
        #endregion
    }

    public enum PedestalState
    {
        IDLE,
        SELECTED,
        DISABLED,
        ACTIVE
    }
}
