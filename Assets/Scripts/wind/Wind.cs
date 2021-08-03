using UnityEngine;
using arrow;

namespace wind {
    public class Wind : MonoBehaviour {
        [SerializeField]
        private float angle;

        private void OnTriggerStay(Collider other) {
            var arrow = other.GetComponent<Arrow>();

            if (arrow == null) {
                return;
            }

            var rigidbody = arrow.GetComponent<Rigidbody>();

            var velocity = rigidbody.velocity;

            float radAngle = angle * Mathf.Deg2Rad;

            float newY = Mathf.Sin(radAngle) * velocity.z + Mathf.Cos(radAngle) * velocity.y;
            float newZ = Mathf.Cos(radAngle) * velocity.z - Mathf.Sin(radAngle) * velocity.y;

            rigidbody.velocity = new Vector3(velocity.x, newY, newZ);
        }
    }
}