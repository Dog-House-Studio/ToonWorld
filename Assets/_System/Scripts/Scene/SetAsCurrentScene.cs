using UnityEngine;
using UnityEngine.SceneManagement;

namespace DogHouse.ToonWorld.Scene
{
    /// <summary>
    /// SetAsCurrentScene will set the current
    /// object's scene as the current active scene.
    /// </summary>
    public class SetAsCurrentScene : MonoBehaviour
    {
        #region Main Methods
        private void LateUpdate()
        {
            SceneManager.SetActiveScene(gameObject.scene);
            this.enabled = false;
        }
        #endregion
    }
}