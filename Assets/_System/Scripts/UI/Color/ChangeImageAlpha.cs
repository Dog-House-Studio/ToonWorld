using UnityEngine;
using UnityEngine.UI;

namespace DogHouse.ToonWorld.UI
{
    /// <summary>
    /// ChangeImageAlpha is a script that changes
    /// this object's image alpha when told to do
    /// so.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class ChangeImageAlpha : MonoBehaviour
    {
        #region Private Variables
        private Image m_image;
        #endregion

        #region Main Methods
        private void Start()
        {
            m_image = GetComponent<Image>();
        }

        public void SetAlpha(float value)
        {
            m_image.SetAlphaTransparency(value);
        }
        #endregion
    }
}
