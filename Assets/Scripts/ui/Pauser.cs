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
        private Bow _bow;
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

            _bow.enabled = false;
        }

        public void Resume() {
            pauseMenu.SetActive(false);
            _bow.enabled = true;

            gameMenu.SetActive(true);
        }
    }
}