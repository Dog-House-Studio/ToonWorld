using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DogHouse.ToonWorld.Map
{
    /// <summary>
    /// MapSliderController is a script that controls
    /// this object's y position using the scroll
    /// wheel. This object slides between two 
    /// objects.
    /// </summary>
    public class MapSliderController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField]
        [Range(0f, 10f)]
        private float m_speed;

        [SerializeField]
        [Range(0f, 1f)]
        private float m_dragSpeed;

        [SerializeField]
        private GameObject m_bottomObject;

        [SerializeField]
        private GameObject m_topObject;

        private float interpolant = 0f;
        Vector3 m_positionCopy;

        private bool m_isDragging = false;
        private Vector3 m_dragStart;
        private float m_startDragInterpolant;
        #endregion

        #region Main Methods
        private void Update()
        {
            if (!m_isDragging)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    m_isDragging = true;
                    m_dragStart = Input.mousePosition;
                    m_startDragInterpolant = interpolant;
                    return;
                }

                interpolant += Input.mouseScrollDelta.y * m_speed * Time.deltaTime;
                interpolant = Mathf.Clamp01(interpolant);
                Move();
                return;
            }

            if(Input.GetMouseButtonUp(0))
            {
                m_isDragging = false;
                return;
            }

            Vector3 diff = Input.mousePosition - m_dragStart;
            float y = diff.y;

            y = y / Screen.height;
            interpolant = m_startDragInterpolant + (-y) * m_dragSpeed;
            Move();
        }
        #endregion

        #region Utility Methods
        private void Move()
        {
            m_positionCopy = this.transform.position;
            m_positionCopy.y =
                Mathf.Lerp(m_bottomObject.transform.position.y,
                m_topObject.transform.position.y, interpolant);
            this.transform.position = m_positionCopy;
        }
        #endregion
    }
}
