using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using bow;

public class Pauser : MonoBehaviour
{
    [SerializeField]
    private Canvas pauseCanvas;
    [SerializeField]
    private Canvas gameCanvas;

    [SerializeField]
    private BowController bowController;
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
        bowController.enabled = false;
        Time.timeScale = 0;
    }

    public void Resume() {
        pauseCanvas.enabled = false;
        bowController.enabled = true;
        gameCanvas.enabled = true;
        Time.timeScale = 1;
    }
}
