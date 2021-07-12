using arrow;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace bow {
    public class BowController : MonoBehaviour {
        [SerializeField]
        private Transform start;
        [SerializeField]
        private Transform max;

        [SerializeField]
        private GameObject arrowRotationPivot;
        [SerializeField]
        private GameObject bowRotationPivot;

        [SerializeField]
        private float minBowRotationAngle;
        [SerializeField]
        private float maxBowRotationAngle;

        [SerializeField]
        private GameObject arrowSpawnGameObject;

        public float pullAmount;
        private Vector3 startTouchPosition;

        public Arrow instantiatedArrow;

        private void Update() {
            if (Input.touchCount <= 0) {
                return;
            }

            if (Input.touches[0].phase == TouchPhase.Began) {

                startTouchPosition = Camera.main.ScreenToViewportPoint(Input.touches[0].position);
                startTouchPosition.z = max.position.z;

                var pos = arrowRotationPivot.transform.position;
                var rot = arrowRotationPivot.transform.rotation;
                var parent = arrowRotationPivot.transform;

                var arrowGameObject = Instantiate(arrowSpawnGameObject, pos, rot, parent);
                instantiatedArrow = arrowGameObject.GetComponentInChildren<Arrow>();
            }


            if (Input.touches[0].phase == TouchPhase.Moved) {

                var touchPosition = Input.touches[0].position;
                Vector3 pullPosition = Camera.main.ScreenToViewportPoint(touchPosition);
                pullPosition.z = max.position.z;

                if (pullPosition.x > startTouchPosition.x) {
                    return;
                }

                var targetPosition = (startTouchPosition - pullPosition).normalized;

                float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;
                angle = Mathf.Clamp(angle, minBowRotationAngle, maxBowRotationAngle);

                bowRotationPivot.transform.rotation = Quaternion.AngleAxis(angle, Vector3.left);

                pullAmount = CalculatePullAmount(pullPosition);

            }

            if (Input.touches[0].phase == TouchPhase.Ended) {

                instantiatedArrow.transform.parent = null;
                instantiatedArrow.Release(pullAmount);

                pullAmount = 0;
                arrowRotationPivot.transform.localPosition = start.localPosition;
                startTouchPosition = Vector3.zero;

            }


        }

        private float CalculatePullAmount(Vector3 pullPosition) {
            var pullVector = pullPosition - startTouchPosition;
            var maxPullVector = max.position - start.position;

            float maxLength = maxPullVector.magnitude;
            float currentLength = pullVector.magnitude;
            float pullAmount = currentLength / maxLength;

            return Mathf.Clamp(pullAmount, 0, 1);
        }
    }
}

