using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using bow;

namespace ui {
    public class Pauser : MonoBehaviour {
        [SerializeField]
        private GameObject pauseMenu;
        [SerializeField]
        private GameObject gameMenu;

        [SerializeField]
        private BowController bowController;
        [SerializeField]
        private TextMeshProUGUI currentLevelText;

        private void Start() {
            pauseMenu.SetActive(false);
            int levelNumber = SceneManager.GetActiveScene().buildIndex;
            currentLevelText.text = $"Level {levelNumber}";
        }

        public void Pause() {
            gameMenu.SetActive(false);
            pauseMenu.SetActive(true);

            bowController.enabled = false;
        }

        public void Resume() {
            pauseMenu.SetActive(false);
            bowController.enabled = true;

            gameMenu.SetActive(true);
        }
    }
}