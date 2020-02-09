using UnityEngine;
using UnityEngine.UI;

namespace DogHouse.ToonWorld.UI
{
    /// <summary>
    /// ExperienceBarSlotController is a script that controls
    /// the visual of a exp bar slot.
    /// </summary>
    public class ExperienceBarSlotController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField]
        private Image m_fillImage;
        #endregion

        #region Main Methods
        public void SetFillColor(Color col)
        {
            col.a = m_fillImage.color.a;
            m_fillImage.color = col;
        }

        public void SetFilled(bool value)
        {
            Color c = m_fillImage.color;

            if(value)
            {
                c.a = 1f;
                m_fillImage.color = c;
                return;
            }

            c.a = 0f;
            m_fillImage.color = c;
        }
        #endregion
    }
}
