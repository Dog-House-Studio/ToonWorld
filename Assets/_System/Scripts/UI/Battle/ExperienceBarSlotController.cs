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
            m_fillImage.color = col;
        }
        #endregion
    }
}
