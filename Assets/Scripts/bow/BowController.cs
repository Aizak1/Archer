using arrow;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace bow {
    public class BowController : MonoBehaviour {
        [SerializeField]
        private Transform minPullTransform;
        [SerializeField]
        private Transform maxPullTransform;

        [SerializeField]
        private GameObject arrowPlacementPoint;
        [SerializeField]
        private GameObject bowRotationPivot;

        [SerializeField]
        private float minBowRotationAngle;
        [SerializeField]
        private float maxBowRotationAngle;

        [SerializeField]
        private GameObject arrowSpawnGameObject;

        [SerializeField]
        private bool isSplitingMode;

        private Vector3 startTouchPosition;

        [HideInInspector]
        public float pullAmount;
        [HideInInspector]
        public Arrow instantiatedArrow;


        private void Update() {
            if (Input.touchCount <= 0) {
                return;
            }

            if (Input.touches[0].phase == TouchPhase.Began) {

                startTouchPosition = Camera.main.ScreenToViewportPoint(Input.touches[0].position);
                startTouchPosition.z = maxPullTransform.position.z;

                var pos = arrowPlacementPoint.transform.position;
                var rot = arrowPlacementPoint.transform.rotation;
                var parent = arrowPlacementPoint.transform;

                var arrowGameObject = Instantiate(arrowSpawnGameObject, pos, rot, parent);
                instantiatedArrow = arrowGameObject.GetComponentInChildren<Arrow>();
            }


            if (Input.touches[0].phase == TouchPhase.Moved) {

                var touchPosition = Input.touches[0].position;
                Vector3 pullPosition = Camera.main.ScreenToViewportPoint(touchPosition);
                pullPosition.z = maxPullTransform.position.z;

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
                var direction = instantiatedArrow.transform.forward;
                var velocity = instantiatedArrow.speed * direction * pullAmount;
                instantiatedArrow.Release(velocity, isSplitingMode);

                pullAmount = 0;
                arrowPlacementPoint.transform.localPosition = minPullTransform.localPosition;
                startTouchPosition = Vector3.zero;

            }

        }

        private float CalculatePullAmount(Vector3 pullPosition) {
            var pullVector = pullPosition - startTouchPosition;
            var maxPullVector = maxPullTransform.position - minPullTransform.position;

            float maxLength = maxPullVector.magnitude;
            float currentLength = pullVector.magnitude;
            float pullAmount = currentLength / maxLength;

            return Mathf.Clamp(pullAmount, 0, 1);
        }
    }
}

