using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace bow
{
    public class BowInput : MonoBehaviour
    {
        private Vector2 _startTouchPosition;
        
        private new Camera _camera;
        private Bow _bow;

        private void Awake()
        {
            _camera = Camera.main;
            _bow = GetComponent<Bow>();
        }

        private void Update()
        {
            if (!_bow.enabled)
            {
                return;
            }
            
            if (Input.GetMouseButtonDown(0)) {
                
                
                var uiObject = EventSystem.current.currentSelectedGameObject;

                if (uiObject && uiObject.GetComponent<Button>()) {
                    return;
                }

                _startTouchPosition = _camera.ScreenToViewportPoint(Input.mousePosition);

                _bow.StartPull();
                
            }
            
            if (Input.GetMouseButton(0)) {

                var touchPosition = Input.mousePosition;
                Vector2 pullPosition = _camera.ScreenToViewportPoint(touchPosition);

                if (pullPosition.x > _startTouchPosition.x) {
                    return;
                }
                
                _bow.Pull(_startTouchPosition, pullPosition);
                
            }
            
            if (Input.GetMouseButtonUp(0)) {

                _bow.EndPull();
            }
        }
    }
}