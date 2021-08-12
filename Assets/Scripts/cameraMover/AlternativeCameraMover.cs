using UnityEngine;

namespace cameraMover {
    public class AlternativeCameraMover : MonoBehaviour {
        [SerializeField]
        private float step;

        [SerializeField]
        private Transform maxPos;

        private Vector3 startPos;

        private void Start() {
            startPos = Camera.main.transform.position;
        }

        public void MoveLeft() {
            var transform = Camera.main.transform.position;
            var pos = new Vector3(transform.x, transform.y, transform.z - step * Time.deltaTime);

            if (pos.z >= startPos.z) {
                Camera.main.transform.position = pos;
            }
        }

        public void MoveRight() {
            var transform = Camera.main.transform.position;
            var pos = new Vector3(transform.x, transform.y, transform.z + step * Time.deltaTime);

            if (pos.z <= maxPos.position.z) {
                Camera.main.transform.position = pos;
            }
        }
    }
}