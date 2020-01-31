using UnityEngine;

namespace DogHouse.ScaffoldCandidates.AssetManagement
{
    /// <summary>
    /// AssetToken is a scriptable object that helps 
    /// to serialize assets and their locations.
    /// </summary>
    [CreateAssetMenu(fileName = "MyNewAssetToken", 
        menuName = "Dog House/Asset Management/Asset Token")]
    public class AssetToken : ScriptableObject
    {
        #region Public Variables
        public Object Asset => m_asset;
        public string AssetName => m_name;
        #endregion

        #region Private Variables
        [SerializeField]
        private UnityEngine.Object m_asset;

        [SerializeField]
        [HideInInspector]
        private string m_name;

        #if UNITY_EDITOR
        [MethodButton("UpdateSerializations")]
        [SerializeField]
        private bool editorFoldout;
        #endif
        #endregion

        private void UpdateSerializations()
        {
            m_name = m_asset.name;
        }
    }
}
