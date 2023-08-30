using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers.Managers
{
    public class LevelSelection : MonoBehaviour
    {
        public void LoadLevel(string levelName)
        {
            SceneManager.LoadScene(levelName);
        }
    }
}