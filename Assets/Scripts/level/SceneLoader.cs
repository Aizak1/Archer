using UnityEngine;
using UnityEngine.SceneManagement;

namespace level {
    public class SceneLoader : MonoBehaviour {
        public void RestartLevel() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadLevel(string name) {
            SceneManager.LoadScene(name);
        }

        public void LoadLevel(int index) {
            SceneManager.LoadScene(index);
        }

        public void LoadNextLevel() {
            var nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if(nextLevelIndex == SceneManager.sceneCountInBuildSettings) {
                nextLevelIndex = 0;
            }
            LoadLevel(nextLevelIndex);
        }

        public void LoadMenu() {
            SceneManager.LoadScene(0);
        }

        public void Quit() {
            Application.Quit();
        }
    }
}

