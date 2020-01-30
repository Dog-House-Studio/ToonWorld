using UnityEngine;

namespace DogHouse.ScaffoldCandidates.GameObjects
{
    /// <summary>
    /// DoNotDestroyOnLoad is a script that when attached
    /// to a gameobject will set that object to not destroy
    /// on load.
    /// </summary>
    public class DoNotDestroyOnLoad : MonoBehaviour
    {
        #region Main Methods
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        #endregion
    }
}
