using enemy;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace arrow {

    public enum ArrowType {
        Normal,
        Fast,
        Slow
    }

    public class Arrow : MonoBehaviour {

        public ArrowType arrowType;

        public float speed;
        public bool isInAir;

        [SerializeField]
        private Transform tip;
        [SerializeField]
        private new Rigidbody rigidbody;

        [SerializeField]
        private int splitArrowsAmount;

        [SerializeField]
        private float timeBeforeSplit;

        [SerializeField]
        private float angleBetweenSplitArrows;

        private float splitTime;
        private bool isSplitArrow;

        private Vector3 lastTipPosition;

        private void Start() {
            lastTipPosition = tip.transform.position;
        }

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

                isInAir = false;

                rigidbody.useGravity = false;
                rigidbody.isKinematic = true;

                var originalLossy = transform.lossyScale;
                transform.parent = hit.collider.gameObject.transform;
                var newLossy = transform.lossyScale;

                var currentLocal = transform.localScale;
                var scaleX = currentLocal.x * originalLossy.x / newLossy.x;
                var scaleY = currentLocal.y * originalLossy.y / newLossy.y;
                var scaleZ = currentLocal.z * originalLossy.z / newLossy.z;

                transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

                var enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null) {
                    enemy.ApplyDamage();
                }

            }

            lastTipPosition = tip.position;

            if (Time.time >= splitTime && isSplitArrow) {
                Split(angleBetweenSplitArrows, splitArrowsAmount);
            }
        }

        private void Split(float angleBetweenSplitArrows, int splitArrowsAmount) {

            float angle = - angleBetweenSplitArrows * (splitArrowsAmount - 1) / 2;

            for (int i = 0; i < splitArrowsAmount; i++) {
                var newArrow = Instantiate(this, transform.position, transform.rotation);
                var velocity = rigidbody.velocity;

                float radAngle = angle * Mathf.Deg2Rad;

                float newY = Mathf.Sin(radAngle) * velocity.z + Mathf.Cos(radAngle) * velocity.y;
                float newZ = Mathf.Cos(radAngle) * velocity.z - Mathf.Sin(radAngle) * velocity.y;

                var newVelocity = new Vector3(velocity.x, newY, newZ);

                newArrow.Release(newVelocity, false);

                angle += angleBetweenSplitArrows;
            }

            Destroy(gameObject);
        }

        public void Release(Vector3 velocity, bool isSplitArrow) {
            isInAir = true;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
            this.isSplitArrow = isSplitArrow;
            rigidbody.velocity = velocity;

            if (isSplitArrow) {
                splitTime = Time.time + timeBeforeSplit;
            }
        }
    }
}

