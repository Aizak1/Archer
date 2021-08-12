using bow;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ui {
    public class TrajectoryShower : MonoBehaviour {

        [SerializeField]
        private BowController bowController;

        [SerializeField]
        private TrajectorySettings trajectorySettup;

        private List<Vector3> pointList;
        private LineRenderer lineRenderer;

        private const string MAIN_TEXTURE = "_MainTex";

        private int index;
        private Vector3 position;

        private void Start() {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 0;
        }

        private void Update() {
            if (Input.GetMouseButton(0) && bowController && bowController.instantiatedArrow) {

                if (Input.GetMouseButtonDown(0)) {

                    var uiObject = EventSystem.current.currentSelectedGameObject;

                    if (uiObject && uiObject.GetComponent<Button>()) {
                        return;
                    }

                    pointList = new List<Vector3>(trajectorySettup.numberOfPoitns);

                    for (int i = 0; i < trajectorySettup.numberOfPoitns; i++) {
                        float spaceBetweenPoints = trajectorySettup.spaceBetweenPoints;
                        var pointPos = CalculatePointPosition(i * spaceBetweenPoints);
                        pointList.Add(pointPos);
                    }

                    if (lineRenderer) {
                        lineRenderer.startWidth = trajectorySettup.StartWidth;
                        lineRenderer.endWidth = trajectorySettup.EndWidth;
                    }

                    for (int i = 1; i < trajectorySettup.numberOfPoitns; i++) {
                        position = pointList[i];
                        lineRenderer.positionCount++;
                        lineRenderer.SetPosition(i - 1, position);
                    }

                    lineRenderer.positionCount++;
                    index = lineRenderer.positionCount - 1;
                    position = pointList[pointList.Count - 1];
                    lineRenderer.SetPosition(index, position);
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

                for (int i = 1; i < trajectorySettup.numberOfPoitns; i++) {
                    var pos = pointList[i];
                    lineRenderer.SetPosition(i - 1, pos);
                }

                index = lineRenderer.positionCount - 1;
                position = pointList[pointList.Count - 1];
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
            ApplySettings();
        }

        private void ApplySettings() {
            lineRenderer.colorGradient = trajectorySettup.Gradient;
            var XTille = trajectorySettup.XTille;
            lineRenderer.material.SetTextureScale(MAIN_TEXTURE, new Vector2(XTille, 1));
        }

        private Vector3 CalculatePointPosition(float t) {
            var arrowTransform = bowController.instantiatedArrow.transform;
            var direction = arrowTransform.forward;
            var vO = bowController.instantiatedArrow.speed * bowController.pullAmount * direction;
            return arrowTransform.position + (vO * t) + (t * t) * 0.5f * Physics.gravity;
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
