using hittable;
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
        public Transform tip;
        [SerializeField]
        private new Rigidbody rigidbody;

        [SerializeField]
        private int splitArrowsAmount;

        [SerializeField]
        private float timeBeforeSplit;

        [SerializeField]
        private float angleBetweenSplitArrows;

        [SerializeField]
        private ParticleSystem splitVfx;

        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip impactSound;
        [SerializeField]
        private AudioClip splitSound;

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
                audioSource.PlayOneShot(impactSound);

                var parent = new GameObject();
                parent.transform.position = hit.collider.gameObject.transform.position;
                parent.transform.rotation = hit.collider.gameObject.transform.rotation;

                parent.transform.parent = hit.collider.gameObject.transform;
                transform.parent = parent.transform;

                var hittable = hit.collider.GetComponent<IHittable>();
                if(hittable != null) {
                    hittable.ProcessHit(this, hit);
                }
                return;
            }

            lastTipPosition = tip.position;

            if (Time.time >= splitTime && isSplitArrow) {
                Split(angleBetweenSplitArrows, splitArrowsAmount);
                Instantiate(splitVfx, transform.position, Quaternion.identity);
            }
        }

        private void Split(float angleBetweenSplitArrows, int splitArrowsAmount) {

            float angle = - angleBetweenSplitArrows * (splitArrowsAmount - 1) / 2;
            Arrow instantiatedArrow = this;
            for (int i = 0; i < splitArrowsAmount; i++) {
                instantiatedArrow = Instantiate(this, transform.position, transform.rotation);
                var velocity = rigidbody.velocity;

                float radAngle = angle * Mathf.Deg2Rad;

                float newY = Mathf.Sin(radAngle) * velocity.z + Mathf.Cos(radAngle) * velocity.y;
                float newZ = Mathf.Cos(radAngle) * velocity.z - Mathf.Sin(radAngle) * velocity.y;

                var newVelocity = new Vector3(velocity.x, newY, newZ);

                instantiatedArrow.Release(newVelocity, false);

                angle += angleBetweenSplitArrows;
            }
            instantiatedArrow.audioSource.PlayOneShot(splitSound);
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

