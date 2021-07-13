using bow;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ui {
    public class TrajectoryShower : MonoBehaviour {

        [SerializeField]
        private GameObject pointPrefab;
        [SerializeField]
        private int numberOfPoitns;
        [SerializeField]
        private float spaceBetweenPoints;

        [SerializeField]
        private BowController bowController;

        private GameObject[] points;

        private void Update() {
            if (Input.touchCount <= 0) {
                return;
            }

            if (Input.touches[0].phase == TouchPhase.Began) {
                points = new GameObject[numberOfPoitns];
                for (int i = 0; i < numberOfPoitns; i++) {
                    var pointPos = CalculatePointPosition(i * spaceBetweenPoints);
                    points[i] = Instantiate(pointPrefab, pointPos , Quaternion.identity);
                }
                points[0].SetActive(false);
            }

            if (Input.touches[0].phase == TouchPhase.Moved) {
                for (int i = 0; i < numberOfPoitns; i++) {
                    points[i].transform.position = CalculatePointPosition(i * spaceBetweenPoints);
                }
            }

            if (Input.touches[0].phase == TouchPhase.Ended) {
                for (int i = 0; i < numberOfPoitns; i++) {
                    Destroy(points[i]);
                }
            }
        }

        private Vector3 CalculatePointPosition(float t) {
            var arrowTransform = bowController.instantiatedArrow.transform;
            var direction = arrowTransform.forward;
            var vO = direction * bowController.pullAmount * bowController.instantiatedArrow.speed;
            return arrowTransform.position + (vO * t) + 0.5f * Physics.gravity * (t * t);
        }
    }
}


