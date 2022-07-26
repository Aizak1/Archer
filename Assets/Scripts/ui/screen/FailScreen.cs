using System;
using TMPro;
using UnityEngine;

namespace ui.screen
{
    public class FailScreen : UiScreen
    {
        [SerializeField] private TextMeshProUGUI _failTimeText;
        [SerializeField] private TextMeshProUGUI _failArrowsCountText;

        private void OnEnable()
        {
            _failTimeText.text = System.Math.Round(_levelController.TimeSinceStart, 2).ToString();
            _failArrowsCountText.text = _levelController.ShotsCount.ToString();
        }
    }
}