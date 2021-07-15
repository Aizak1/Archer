using bow;
using arrow;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

namespace ui {
    public class UiController : MonoBehaviour {
        [SerializeField]
        private Image touchMarkUp;
        [SerializeField]
        private BowController bowController;

        private void Update() {

            if (Input.GetMouseButtonDown(0)) {

                if (Input.touchCount > 0) {
                    int id = Input.touches[0].fingerId;
                    if (EventSystem.current.IsPointerOverGameObject(id)) {
                        return;
                    }
                } else {
                    if (EventSystem.current.IsPointerOverGameObject()) {
                        return;
                    }
                }

                touchMarkUp.rectTransform.position = Input.mousePosition;
                touchMarkUp.gameObject.SetActive(true);
            }

            if (Input.GetMouseButtonUp(0)) {
                touchMarkUp.gameObject.SetActive(false);
            }
        }

        public void SwitchArrowTypeButton() {
            var nextArrowType = bowController.instantiatedArrowType + 1;
            if((int)nextArrowType == Enum.GetNames(typeof(ArrowType)).Length) {
                nextArrowType = 0;
            }
            bowController.instantiatedArrowType = nextArrowType;
        }
    }
}

