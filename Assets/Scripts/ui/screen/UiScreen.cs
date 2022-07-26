using System;
using System.Runtime.CompilerServices;
using level;
using UnityEngine;

namespace ui.screen
{
    public abstract class UiScreen: MonoBehaviour
    {
        [SerializeField] private GameState _activationState;
        protected LevelController _levelController;
        

        public void Init(LevelController levelController)
        {
            _levelController = levelController;
            _levelController.OnGameStateChanged.AddListener(SetActive);
        }

        private void SetActive(GameState gameState)
        {
            gameObject.SetActive(_activationState == gameState);
        }
        
    }
}