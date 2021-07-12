using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ui {
    public class UiController : MonoBehaviour {
        [SerializeField]
        private Image touchMarkUp;

        private void Update() {
            if (Input.touchCount <= 0) {
                return;
            }

            if (Input.touches[0].phase == TouchPhase.Began) {
                touchMarkUp.rectTransform.position = Input.touches[0].position;
                touchMarkUp.gameObject.SetActive(true);
            }

            if (Input.touches[0].phase == TouchPhase.Ended) {
                touchMarkUp.gameObject.SetActive(false);
            }
        }
    }
}

