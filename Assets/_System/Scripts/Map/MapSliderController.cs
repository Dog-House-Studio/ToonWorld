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
        private GameObject m_bottomObject;

        [SerializeField]
        private GameObject m_topObject;

        private float interpolant = 0f;
        Vector3 m_positionCopy;
        #endregion

        #region Main Methods
        private void Update()
        {
            interpolant += Input.mouseScrollDelta.y * m_speed * Time.deltaTime;
            interpolant = Mathf.Clamp01(interpolant);


            m_positionCopy = this.transform.position;
            m_positionCopy.y = 
                Mathf.Lerp(m_bottomObject.transform.position.y, 
                m_topObject.transform.position.y, interpolant);

            this.transform.position = m_positionCopy;
        }
        #endregion
    }
}
