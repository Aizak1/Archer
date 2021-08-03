using UnityEngine;
using UnityEngine.UI;


namespace cameraMover {
    public class CameraMover : MonoBehaviour {
        private Vector3 minPosition;

        [SerializeField]
        private Transform max;

        [SerializeField]
        private Slider slider;

        private void Start() {
            minPosition = Camera.main.transform.position;
        }

        private void Update() {
            var percent = slider.value;

            var deltaPos = (max.position - minPosition) * percent;
            Camera.main.transform.position = minPosition + deltaPos;
        }
    }
}

