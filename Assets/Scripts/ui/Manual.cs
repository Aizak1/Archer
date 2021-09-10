using UnityEngine;
using level;
using bow;
using UnityEngine.SceneManagement;

namespace ui {
    public class Manual : MonoBehaviour {
        [SerializeField]
        private GameObject gameMenu;

        [SerializeField]
        private BowController bowController;

        [SerializeField]
        private GameObject[] manualMenues;
        [SerializeField]
        private int minManualIndex;
        private int currentIndex;

        [SerializeField]
        private bool isQuiz;

        [SerializeField]
        private HintController hintController;

        private void Start() {
            if (isQuiz) {
                return;
            }

            int currentScene = SceneManager.GetActiveScene().buildIndex;
            if (PlayerPrefs.GetInt(LevelsManager.LEVEL_AT) > currentScene) {
                return;
            }

            ShowManual();
        }

        public void CloseManual() {
            manualMenues[currentIndex].SetActive(false);
            bowController.enabled = true;

            if (hintController) {
                hintController.gameMenu = gameMenu;
                hintController.enabled = true;
            } else {
                gameMenu.SetActive(true);
            }

        }

        public void ShowNextManual() {
            if (currentIndex >= manualMenues.Length - 1) {
                return;
            }
            CloseManual();
            currentIndex++;
            ShowManual();
        }

        public void ShowPreviousManual() {
            if (currentIndex <= minManualIndex) {
                return;
            }
            CloseManual();
            currentIndex--;
            ShowManual();
        }

        public void ShowManual() {
            if (manualMenues.Length == 0) {
                return;
            }

            if(currentIndex >= manualMenues.Length) {
                return;
            }

            if (currentIndex < 0) {
                return;
            }

            manualMenues[currentIndex].SetActive(true);
            gameMenu.SetActive(false);

            bowController.enabled = false;
        }
    }
}