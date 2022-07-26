using System;
using DG.Tweening;
using level;
using TMPro;
using UnityEngine;

namespace ui.screen
{
    public class WinScreen : UiScreen
    {
        [SerializeField] private Star[] _stars;
        [SerializeField] private TextMeshProUGUI _timeText;
        [SerializeField] private TextMeshProUGUI _arrowsCountText;
        
        [SerializeField] private float _starScaleSpeed = 0.5f;

        private readonly Vector3 _starScale = new Vector3(1.12f, 1.36f, 1);

        private void OnEnable()
        {
            _timeText.text = System.Math.Round(_levelController.TimeSinceStart, 2).ToString();
            _arrowsCountText.text = _levelController.ShotsCount.ToString();
            
            for (int i = 0; i < _levelController.StarConditionsCompleteCount; i++) {
                _stars[i].gameObject.SetActive(true);
                var transform = _stars[i].rectTransform;
                transform.localScale = Vector3.zero;
                transform.DOScale(_starScale, _starScaleSpeed);
            }
        }
    }
}