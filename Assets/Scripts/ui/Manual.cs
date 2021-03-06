using UnityEngine;
using level;
using bow;
using UnityEngine.SceneManagement;

namespace ui {
    public class Manual : MonoBehaviour {
        [SerializeField]
        private Canvas gameCanvas;

        [SerializeField]
        private BowController bowController;

        [SerializeField]
        private Canvas[] manualCanvases;
        private int currentIndex;

        [SerializeField]
        private HintController hintController;

        private void Start() {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            if (PlayerPrefs.GetInt(LevelsManager.LEVEL_AT) > currentScene) {
                return;
            }

            if (manualCanvases.Length == 0) {
                return;
            }

            currentIndex = 0;

            manualCanvases[currentIndex].enabled = true;
            gameCanvas.enabled = false;

            bowController.enabled = false;
        }

        public void CloseManual() {
            manualCanvases[currentIndex].enabled = false;

            currentIndex++;

            if (currentIndex >= manualCanvases.Length) {
                bowController.enabled = true;

                if (hintController) {
                    hintController.gameCanvas = gameCanvas;
                    hintController.enabled = true;
                } else {
                    gameCanvas.enabled = true;
                }
            } else {
                manualCanvases[currentIndex].enabled = true;
            }
        }
    }
}