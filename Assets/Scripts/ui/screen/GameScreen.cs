using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ui.screen
{
    public class GameScreen : UiScreen
    {
        [SerializeField] private TextMeshProUGUI _targetsCountText;
        [SerializeField] private TextMeshProUGUI _arrowTypeText;
        
        [SerializeField] private RawImage _arrowImage;
        
        [SerializeField] private Texture[] _arrowImageTextures;
        
        [SerializeField] private TextMeshProUGUI _spendedArrowsText;
       
        [SerializeField] private TextMeshProUGUI _timeText;

        [SerializeField] private Button _pauseButton;
        
        private string _arrowsForStar;
        
        private float _targetTime;
        private float _currentTime;
        private int _timeRemain;

        private void Start() 
        {
            _arrowsForStar = _levelController.CountOfArrowsForStar.ToString();
            ShowTargetsCountText();
            ShowSpendArrows();
            
            _levelController.OnTargetsDecrease.AddListener(ShowTargetsCountText);
            _levelController.Bow.OnEndPull += ShowSpendArrows;
            
            _pauseButton.onClick.AddListener(_levelController.Pause);
        }
        private void Update() 
        {
            _targetTime = _levelController.StarTime;
            _currentTime = _levelController.TimeSinceStart;
            _timeRemain = Mathf.RoundToInt(_targetTime - _currentTime);

            if (_timeRemain <= 0) 
            {
                _timeText.text = "0";
                _timeText.color = Color.red;
                enabled = false;
            } 
            else 
            {
                _timeText.text = "";
                _timeText.text += _timeRemain;
            }
            
        }

        private void ShowTargetsCountText()
        {
            _targetsCountText.text = _levelController.TargetsCount.ToString();
        }

        private void ShowSpendArrows()
        {
            _spendedArrowsText.text = _levelController.ShotsCount + " / " + _arrowsForStar;
        }

        private void OnDestroy()
        {
            _levelController.Bow.OnEndPull -= ShowSpendArrows;
        }
    }
}