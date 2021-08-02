using bow;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ui {
    public class TrajectoryShower : MonoBehaviour {

        [SerializeField]
        private GameObject pointPrefab;
        [SerializeField]
        private int numberOfPoitns;
        [SerializeField]
        private float spaceBetweenPoints;

        private readonly Vector3 MIN_TRAJECTORY_OBJECT_SIZE = new Vector3(0.025f, 0.025f, 0.025f);

        [SerializeField]
        private BowController bowController;

        private GameObject[] points;

        private LineRenderer lineRenderer;

        [SerializeField]
        private Color startColor;
        private Color endColor;

        [SerializeField]
        private float endAlpha;

        private void Start() {
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 0;
            endColor = new Color(startColor.r, startColor.g, startColor.b, endAlpha);
        }

        private void LateUpdate() {

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

                    points = new GameObject[numberOfPoitns];
                    var maxScale = pointPrefab.transform.localScale;
                    var scaleStep = (maxScale - MIN_TRAJECTORY_OBJECT_SIZE) / numberOfPoitns;

                    for (int i = 0; i < numberOfPoitns; i++) {
                        var pointPos = CalculatePointPosition(i * spaceBetweenPoints);
                        points[i] = Instantiate(pointPrefab, pointPos, Quaternion.identity);

                        if (i > 0 && points[i - 1] != null) {
                            var scale = points[i - 1].transform.localScale - scaleStep;
                            points[i].transform.localScale = scale;
                        }

                    }

                    points[0].SetActive(false);
                    if (lineRenderer) {
                        lineRenderer.startWidth = points[0].transform.localScale.x;
                        lineRenderer.endWidth = points[numberOfPoitns - 1].transform.localScale.x;

                        lineRenderer.startColor = startColor;
                        lineRenderer.endColor = endColor;

                    }

                }

                if (points == null) {
                    return;
                }

                for (int i = 0; i < lineRenderer.positionCount; i++) {
                    points[i].transform.position = CalculatePointPosition(i * spaceBetweenPoints);
                }

                if (!lineRenderer) {
                    return;
                }

                lineRenderer.positionCount = 0;
                lineRenderer.transform.position = points[1].transform.position;
                for (int i = 1; i < numberOfPoitns; i++) {
                    var pos = points[i].transform.position;
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(i - 1, pos);
                }
                lineRenderer.positionCount++;
                var index = lineRenderer.positionCount - 1;
                var position = points[points.Length - 1].transform.position;
                lineRenderer.SetPosition(index, position);
            }

            if (Input.GetMouseButtonUp(0)) {

                if (points == null) {
                    return;
                }

                for (int i = 0; i < numberOfPoitns; i++) {
                    Destroy(points[i]);
                }

                points = null;

                if (lineRenderer) {
                    lineRenderer.positionCount = 0;
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


