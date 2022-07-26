using System;
using level;
using UnityEngine;

namespace ui.screen
{
    public class UiScreenController : MonoBehaviour
    {
        [SerializeField] private UiScreen[] _screens;
        [SerializeField] private LevelController _levelController;

        private void Awake()
        {
            foreach (var item in _screens)
            {
                item.Init(_levelController);
            }
        }
    }
}