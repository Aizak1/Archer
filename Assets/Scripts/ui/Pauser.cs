using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using bow;
using ui;

public class Pauser : MonoBehaviour
{
    [SerializeField]
    private Canvas pauseCanvas;
    [SerializeField]
    private Canvas gameCanvas;

    [SerializeField]
    private BowController bowController;
    [SerializeField]
    private TrajectoryShower trajectoryShower;
    [SerializeField]
    private TextMeshProUGUI currentLevelText;

    private void Start() {
        pauseCanvas.enabled = false;
        int levelNumber = SceneManager.GetActiveScene().buildIndex;
        currentLevelText.text = $"Level {levelNumber}";

    }

    public void Pause() {
        gameCanvas.enabled = false;
        pauseCanvas.enabled = true;

        trajectoryShower.enabled = false;
        bowController.enabled = false;
    }

    public void Resume() {
        pauseCanvas.enabled = false;
        bowController.enabled = true;

        gameCanvas.enabled = true;
        trajectoryShower.enabled = true;
    }
}
