using bow;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ui {
    public class TrajectoryShower : MonoBehaviour {

        [SerializeField]
        private BowController bowController;

        [SerializeField]
        private TrajectorySettings trajectorySettup;

        private Vector3[] pointList;
        private LineRenderer lineRenderer;

        private const string MAIN_TEXTURE = "_MainTex";

        private void Start() {
            lineRenderer = GetComponent<LineRenderer>();

            if (!lineRenderer) {
                Debug.LogError("No LineRenderer on trajectory shower");
                return;
            }

            lineRenderer.positionCount = 0;
        }
        public void StartDraw() {
            if (!lineRenderer) {
                return;
            }

            var uiObject = EventSystem.current.currentSelectedGameObject;

            if (uiObject && uiObject.GetComponent<Button>()) {
                return;
            }

            lineRenderer.startWidth = trajectorySettup.StartWidth;
            lineRenderer.endWidth = trajectorySettup.EndWidth;
            lineRenderer.positionCount = trajectorySettup.numberOfPoitns;
        }

        public void Draw() {
            if (!lineRenderer) {
                return;
            }

            for (int i = 0; i < pointList.Length; i++) {
                var time = (i + 1) * trajectorySettup.spaceBetweenPoints;
                pointList[i] = CalculatePointPosition(time);
            }

            for (int i = 0; i < trajectorySettup.numberOfPoitns; i++) {
                var pos = pointList[i];
                lineRenderer.SetPosition(i, pos);
            }
        }

        public void EndDraw() {
            if (!lineRenderer) {
                return;
            }

            lineRenderer.positionCount = 0;
        }

        public void SetSettings(TrajectorySettings settings) {
            trajectorySettup = settings;
            ApplySettings();
        }

        private void ApplySettings() {
            if (!lineRenderer) {
                return;
            }

            lineRenderer.colorGradient = trajectorySettup.Gradient;
            var XTille = trajectorySettup.XTille;
            lineRenderer.material.SetTextureScale(MAIN_TEXTURE, new Vector2(XTille, 1));
            pointList = new Vector3[trajectorySettup.numberOfPoitns];
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
