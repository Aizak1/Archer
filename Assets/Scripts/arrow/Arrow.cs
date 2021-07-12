using enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace arrow {
    public class Arrow : MonoBehaviour {
        public float speed;
        public bool isInAir;

        [SerializeField]
        private Transform tip;
        [SerializeField]
        private new Rigidbody rigidbody;

        private Vector3 lastTipPosition = Vector3.zero;

        private void FixedUpdate() {
            if (!isInAir) {
                return;
            }

            if (rigidbody.velocity == Vector3.zero) {
                return;
            }

            transform.rotation = Quaternion.LookRotation(rigidbody.velocity, transform.up);

            if (Physics.Linecast(lastTipPosition, tip.position, out RaycastHit hit)) {
                rigidbody.Sleep();
                rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

                isInAir = false;

                rigidbody.useGravity = false;
                rigidbody.isKinematic = true;
                GetComponent<Collider>().enabled = false;

                transform.parent = hit.collider.gameObject.transform;
                var enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null) {
                    enemy.ApplyDamage();
                }

            }
            lastTipPosition = tip.position;
        }

        public void Release(float pullAmount) {
            isInAir = true;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;

            Vector3 force = transform.forward * (pullAmount * speed);
            rigidbody.velocity = force;

        }
    }
}

