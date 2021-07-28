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

        [SerializeField]
        private BowController bowController;

        private GameObject[] points;

        private void LateUpdate() {

            if (Input.GetMouseButton(0)) {

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

                    for (int i = 0; i < numberOfPoitns; i++) {
                        var pointPos = CalculatePointPosition(i * spaceBetweenPoints);
                        points[i] = Instantiate(pointPrefab, pointPos, Quaternion.identity);
                    }

                    points[0].SetActive(false);
                }

                if (points == null) {
                    return;
                }

                for (int i = 0; i < numberOfPoitns; i++) {
                    points[i].transform.position = CalculatePointPosition(i * spaceBetweenPoints);
                }
            }

            if (Input.GetMouseButtonUp(0)) {

                if (points == null) {
                    return;
                }

                for (int i = 0; i < numberOfPoitns; i++) {
                    Destroy(points[i]);
                }

                points = null;
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


