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
        [SerializeField]
        private ArrowResource resource;

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
            ArrowType currentArrowType = bowController.arrowTypeToInstantiate;
            int currentTypeIndex = resource.arrowTypeToCount[currentArrowType];
            int nextTypeIndex = currentTypeIndex + 1;

            if(nextTypeIndex == resource.arrowTypeToCount.Count) {
                nextTypeIndex = 0;
            }

            ArrowType nextType = resource.countToArrowType[nextTypeIndex];
            bowController.arrowTypeToInstantiate = nextType;
        }
    }
}

