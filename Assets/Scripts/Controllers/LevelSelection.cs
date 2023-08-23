using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class LevelSelection : MonoBehaviour
    {
        public void LoadLevel(string levelName)
        {
            SceneManager.LoadScene(levelName);
        }
    }
}