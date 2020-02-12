using UnityEngine;
using TMPro;

namespace DogHouse.ToonWorld.UI
{
    /// <summary>
    /// NumberEffectController is a script that controls
    /// the number effect. Other objects will set the value
    /// of the number. When this object is finished animating,
    /// it will be destroyed.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class NumberEffectController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField]
        private TMP_Text m_text;

        [SerializeField]
        private Color m_damageColor;

        [SerializeField]
        private Color m_healColor;

        private Animator m_animator;

        private const string ANIMATOR_END_STATE = "End";
        #endregion

        #region Main Methods
        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        public void SetValue(int value)
        {
            m_text.text = value.ToString();
            if (value < 0) m_text.color = m_damageColor;
            if (value > 0) m_text.color = m_healColor;
        }

        public void Update()
        {
            if(m_animator.GetCurrentAnimatorStateInfo(0).IsName(ANIMATOR_END_STATE))
            {
                Destroy(this.gameObject);
            }
        }
        #endregion
    }
}
