using arrow;
using UnityEngine;
using UnityEngine.EventSystems;
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
        private ArrowResource arrowResource;

        [SerializeField]
        private bool isSplitingMode;

        private Vector3 startTouchPosition;

        [HideInInspector]
        public float pullAmount;
        [HideInInspector]
        public ArrowType instantiatedArrowType;
        [HideInInspector]
        public Arrow instantiatedArrow;


        private void Update() {
            if (Input.touchCount <= 0) {
                return;
            }

            if (Input.touches[0].phase == TouchPhase.Began) {

                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
                    return;
                }

                startTouchPosition = Camera.main.ScreenToViewportPoint(Input.touches[0].position);
                startTouchPosition.z = maxPullTransform.position.z;

                var pos = arrowPlacementPoint.transform.position;
                var rot = arrowPlacementPoint.transform.rotation;
                var parent = arrowPlacementPoint.transform;

                var arrowGameObjectToSpawn = arrowResource.arrowPrefabs[instantiatedArrowType];
                var arrowGameObject = Instantiate(arrowGameObjectToSpawn, pos, rot, parent);
                instantiatedArrow = arrowGameObject.GetComponentInChildren<Arrow>();
            }

            if (instantiatedArrow == null) {
                return;
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
                instantiatedArrow = null;

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

