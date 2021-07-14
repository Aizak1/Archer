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
            if (Input.touchCount <= 0) {
                return;
            }

            if (Input.touches[0].phase == TouchPhase.Began) {

                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
                    return;
                }

                touchMarkUp.rectTransform.position = Input.touches[0].position;
                touchMarkUp.gameObject.SetActive(true);
            }

            if (Input.touches[0].phase == TouchPhase.Ended) {
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

