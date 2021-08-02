using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using level;
using bow;
using UnityEngine.SceneManagement;

namespace ui {
    public class Manual : MonoBehaviour {
        [SerializeField]
        private Canvas manualCanvas;
        [SerializeField]
        private Canvas gameCanvas;

        [SerializeField]
        private BowController bowController;
        [SerializeField]
        private TrajectoryShower trajectoryShower;



        private void Start() {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            if (PlayerPrefs.GetInt(LevelsManager.LEVEL_AT) <= currentScene) {
                manualCanvas.enabled = true;
                gameCanvas.enabled = false;

                trajectoryShower.enabled = false;
                bowController.enabled = false;
            }
        }

        public void CloseManual() {
            manualCanvas.enabled = false;
            gameCanvas.enabled = true;
            trajectoryShower.enabled = true;
            bowController.enabled = true;
        }
    }
}

