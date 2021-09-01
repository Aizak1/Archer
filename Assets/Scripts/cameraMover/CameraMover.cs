using UnityEngine;
using UnityEngine.UI;


namespace cameraMover {
    public class CameraMover : MonoBehaviour {
        private Vector3 minPosition;

        [SerializeField]
        private Transform max;

        [SerializeField]
        private Slider slider;

        [SerializeField]
        private new Camera camera;

        private void Start() {
            minPosition = camera.transform.position;
        }

        private void Update() {
            var percent = slider.value;

            var deltaPos = (max.position - minPosition) * percent;
            camera.transform.position = minPosition + deltaPos;
        }
    }
}

