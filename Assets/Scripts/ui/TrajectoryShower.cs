using bow;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ui {
    public class TrajectoryShower : MonoBehaviour {

        [SerializeField]
        private BowController bowController;

        [SerializeField]
        private TrajectorySettings trajectorySettup;

        private List<Vector3> pointList;
        private LineRenderer lineRenderer;

        private const string MAIN_TEXTURE = "_MainTex";
        private void Start() {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 0;
        }

        private void Update() {
            if (Input.GetMouseButton(0) && bowController && bowController.instantiatedArrow) {

                if (Input.GetMouseButtonDown(0)) {

                    if (Input.touchCount > 0) {
                        int id = Input.touches[0].fingerId;
                        if (EventSystem.current.IsPointerOverGameObject(id)) {
                            return;
                        }
                    } else {
                        if (EventSystem.current.IsPointerOverGameObject()) {
                            return;
                        }
                    }

                    pointList = new List<Vector3>();

                    for (int i = 0; i < trajectorySettup.numberOfPoitns; i++) {
                        float spaceBetweenPoints = trajectorySettup.spaceBetweenPoints;
                        var pointPos = CalculatePointPosition(i * spaceBetweenPoints);
                        pointList.Add(pointPos);
                    }

                    if (lineRenderer) {
                        lineRenderer.startWidth = trajectorySettup.StartWidth;
                        lineRenderer.endWidth = trajectorySettup.EndWidth;
                    }
                }

                if (pointList == null) {
                    return;
                }

                for (int i = 0; i < pointList.Count; i++) {
                    pointList[i] = CalculatePointPosition(i * trajectorySettup.spaceBetweenPoints);
                }

                if (!lineRenderer) {
                    return;
                }

                lineRenderer.positionCount = 0;
                for (int i = 1; i < trajectorySettup.numberOfPoitns; i++) {
                    var pos = pointList[i];
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(i - 1, pos);
                }

                lineRenderer.positionCount++;
                var index = lineRenderer.positionCount - 1;
                var position = pointList[pointList.Count - 1];
                lineRenderer.SetPosition(index, position);
            }

            if (Input.GetMouseButtonUp(0)) {

                if (pointList == null) {
                    return;
                }
                pointList = null;

                if (lineRenderer) {
                    lineRenderer.positionCount = 0;
                }
            }
        }

        public void SetSettings(TrajectorySettings settings) {
            trajectorySettup = settings;
            AplySettings();
        }

        private void AplySettings() {
            lineRenderer.colorGradient = trajectorySettup.Gradient;
            var XTille = trajectorySettup.XTille;
            lineRenderer.material.SetTextureScale(MAIN_TEXTURE, new Vector2(XTille, 1));
        }

        private Vector3 CalculatePointPosition(float t) {
            var arrowTransform = bowController.instantiatedArrow.transform;
            var direction = arrowTransform.forward;
            var vO = direction * bowController.pullAmount * bowController.instantiatedArrow.speed;
            return arrowTransform.position + (vO * t) + 0.5f * Physics.gravity * (t * t);
        }
    }

    [Serializable]
    public struct TrajectorySettings {
        public Gradient Gradient;
        public float XTille;
        public float StartWidth;
        public float EndWidth;
        public float spaceBetweenPoints;
        public int numberOfPoitns;
    }
}
