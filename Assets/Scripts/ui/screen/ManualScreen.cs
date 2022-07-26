using System;
using UnityEngine;
using level;
using bow;
using ui.screen;
using UnityEngine.SceneManagement;

namespace ui {
    public class ManualScreen : UiScreen {
        
        [SerializeField] private ManualObject[] _manualMenues;
        [SerializeField] private HintScreen _hintScreen;
        private int _currentIndex;
        
        private void Start() {
            
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            if (PlayerPrefs.GetInt(LevelsManager.LEVEL_AT) > currentScene) 
            {
                Debug.Log("Change from manual");
                _levelController.ChangeGameState(GameState.InGame);
                return;
            }
            if (_manualMenues.Length == 0) 
            {
                _levelController.ChangeGameState(GameState.InGame);
                return;
            }
            
            foreach (var item in _manualMenues)
            {
                item.CloseButton.onClick.AddListener(CloseManual);
            }

            _currentIndex = 0;
            _manualMenues[_currentIndex].gameObject.SetActive(true);

        }

        public void CloseManual() 
        {
            _manualMenues[_currentIndex].gameObject.SetActive(false);
            _currentIndex++;

            if (_currentIndex >= _manualMenues.Length)
            {
                _levelController.ChangeGameState(_hintScreen ? GameState.Hint : GameState.InGame);
            } 
            else 
            {
                _manualMenues[_currentIndex].gameObject.SetActive(true);
            }
        }
    }
}