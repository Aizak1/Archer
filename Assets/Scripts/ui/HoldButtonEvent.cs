using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ui {
    public class HoldButtonEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

        [SerializeField]
        private UnityEvent unityEvent;

        [SerializeField]
        private Image image;

        [SerializeField]
        private Color holdColor;

        private Color originalColor;

        private bool isHold = false;

        private void Awake() {
            originalColor = image.color;
        }

        public void OnPointerDown(PointerEventData eventData) {
            image.color = holdColor;
            isHold = true;
        }

        public void OnPointerUp(PointerEventData eventData) {
            image.color = originalColor;
            isHold = false;
        }

        private void Update() {
            if (!isHold) {
                return;
            }
            unityEvent.Invoke();
        }
    }
}
