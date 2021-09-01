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
        [SerializeField]
        private LineRenderer lineRenderer;

        private const string MAIN_TEXTURE = "_MainTex";

        [SerializeField]
        private GameObject[] balls;
        [SerializeField]
        private float ballDistance = 0.4f;
        private int ballindex;

        private void Start() {
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

            if (balls.Length == 0) {
                return;
            }

            float[] magnitudes = new float[pointList.Length - 1];
            float sum = 0;

            for (int i = 0; i < magnitudes.Length; i++) {
                magnitudes[i] = (pointList[i] - pointList[i + 1]).magnitude;
                sum += magnitudes[i];
            }

            float requiredMag = ballDistance;
            float tmpMag = 0;
            ballindex = 0;

            for (int i = 0; i < magnitudes.Length; i++) {
                if (tmpMag >= sum) {
                    break;
                }

                if (tmpMag + magnitudes[i] >= requiredMag) {
                    float need = requiredMag - tmpMag;
                    float full = magnitudes[i];

                    float percent = need / full;

                    Vector3 line = pointList[i + 1] - pointList[i];

                    balls[ballindex].transform.position = pointList[i] + line * percent;
                    balls[ballindex].SetActive(true);
                    ballindex++;

                    tmpMag += magnitudes[i];
                    requiredMag += ballDistance;
                } else {
                    tmpMag += magnitudes[i];
                }
            }

            for (int i = ballindex; i < balls.Length; i++) {
                balls[i].SetActive(false);
            }

            if (ballindex == 2) {
                balls[0].transform.localScale = Vector3.one * trajectorySettup.StartWidth;
                balls[1].transform.localScale = Vector3.one * trajectorySettup.EndWidth;
                return;
            } else if (ballindex == 1) {
                balls[0].transform.localScale = Vector3.one * trajectorySettup.StartWidth;
                return;

            } else if (ballindex - 2 < 0) {
                return;
            }

            float deltaWidth = (trajectorySettup.StartWidth - trajectorySettup.EndWidth);
            float scaleStep = deltaWidth / (ballindex - 1);
            float scale = trajectorySettup.StartWidth;

            for (int i = 0; i < ballindex; i++) {
                balls[i].transform.localScale = Vector3.one * scale;
                scale -= scaleStep;
            }
        }

        public void EndDraw() {
            if (!lineRenderer) {
                return;
            }

            lineRenderer.positionCount = 0;

            if (balls.Length == 0) {
                return;
            }

            for (int i = 0; i <= ballindex; i++) {
                if (i >= balls.Length) {
                    return;
                }
                balls[i].SetActive(false);
            }
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