using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace bow
{
    public class BowInput : MonoBehaviour
    {
        [SerializeField] private Transform _minPullTransform;
        [SerializeField] private Transform _maxPullTransform;
        
        private Vector3 _startTouchPosition;
        
        private new Camera _camera;
        private Bow _bow;

        private void Awake()
        {
            _camera = Camera.main;
            _bow = GetComponent<Bow>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) {

                var uiObject = EventSystem.current.currentSelectedGameObject;

                if (uiObject && uiObject.GetComponent<Button>()) {
                    return;
                }

                _startTouchPosition = _camera.ScreenToViewportPoint(Input.mousePosition);
                _startTouchPosition.z = _maxPullTransform.position.z;
                
                _bow.StartPull();
                
            }
            
            if (Input.GetMouseButton(0)) {

                var touchPosition = Input.mousePosition;
                Vector3 pullPosition = _camera.ScreenToViewportPoint(touchPosition);
                pullPosition.z = _maxPullTransform.position.z;

                if (pullPosition.x > _startTouchPosition.x) {
                    return;
                }
                
                _bow.Pull(_startTouchPosition, pullPosition, _maxPullTransform.position,_minPullTransform.position);
                
            }
            
            if (Input.GetMouseButtonUp(0)) {

                _bow.EndPull();
            }
        }
    }
}