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
        private int currentIndex;

        [SerializeField]
        private HintController hintController;

        private void Start() {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            if (PlayerPrefs.GetInt(LevelsManager.LEVEL_AT) > currentScene) {
                return;
            }

            if (manualMenues.Length == 0) {
                return;
            }

            currentIndex = 0;

            manualMenues[currentIndex].SetActive(true);
            gameMenu.SetActive(false);

            bowController.enabled = false;
        }

        public void CloseManual() {
            manualMenues[currentIndex].SetActive(false);

            currentIndex++;

            if (currentIndex >= manualMenues.Length) {
                bowController.enabled = true;

                if (hintController) {
                    hintController.gameMenu = gameMenu;
                    hintController.enabled = true;
                } else {
                    gameMenu.SetActive(true);
                }
            } else {
                manualMenues[currentIndex].SetActive(true);
            }
        }
    }
}