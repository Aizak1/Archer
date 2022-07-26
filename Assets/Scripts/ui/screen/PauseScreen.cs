using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using bow;
using ui.screen;
using UnityEngine.UI;

namespace ui.screen {
    public class PauseScreen : UiScreen {
        
        [SerializeField] private TextMeshProUGUI currentLevelText;
        [SerializeField] private Button _resumeButton;

        private void OnEnable() {
            int levelNumber = SceneManager.GetActiveScene().buildIndex;
            currentLevelText.text = $"Level {levelNumber}";
            _resumeButton.onClick.AddListener(_levelController.Resume);
        }
    }
}