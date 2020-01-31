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
        

        [SerializeField]
        private UnityEngine.Object m_asset;

#if UNITY_EDITOR
        [MethodButton("UpdateSerializations")]
        [SerializeField]
        private bool editorFoldout;
#endif

        private void UpdateSerializations()
        {

        }
    }
}
