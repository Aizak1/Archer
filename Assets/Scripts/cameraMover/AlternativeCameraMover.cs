using UnityEngine;

namespace cameraMover {
    public class AlternativeCameraMover : MonoBehaviour {
        [SerializeField]
        private float step;

        [SerializeField]
        private Transform maxPos;

        private Vector3 startPos;

        private new Camera camera;

        private void Start() {
            camera = Camera.main;
            startPos = camera.transform.position;
        }

        public void MoveLeft() {
            var transform = camera.transform.position;
            var pos = new Vector3(transform.x, transform.y, transform.z - step * Time.deltaTime);

            if (pos.z >= startPos.z) {
                camera.transform.position = pos;
            }
        }

        public void MoveRight() {
            var transform = camera.transform.position;
            var pos = new Vector3(transform.x, transform.y, transform.z + step * Time.deltaTime);

            if (pos.z <= maxPos.position.z) {
                camera.transform.position = pos;
            }
        }
    }
}