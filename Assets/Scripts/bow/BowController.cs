using arrow;
using UnityEngine;
using UnityEngine.EventSystems;

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
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip shootSound;
        [SerializeField]
        private AudioClip pullingSound;

        [SerializeField]
        private bool isSplitingMode;

        private Vector3 startTouchPosition;

        [HideInInspector]
        public float pullAmount;
        [HideInInspector]
        public ArrowType arrowTypeToInstantiate;
        [HideInInspector]
        public Arrow instantiatedArrow;

        private new Camera camera;

        private void Start() {
            arrowTypeToInstantiate = arrowResource.countToArrowType[0];
            camera = Camera.main;
        }

        private void Update() {

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

                    startTouchPosition = camera.ScreenToViewportPoint(Input.mousePosition);
                    startTouchPosition.z = maxPullTransform.position.z;

                    var pos = arrowPlacementPoint.transform.position;
                    var rot = arrowPlacementPoint.transform.rotation;
                    var parent = arrowPlacementPoint.transform;

                    var arrowObjectToSpawn = arrowResource.arrowPrefabs[arrowTypeToInstantiate];
                    var arrowGameObject = Instantiate(arrowObjectToSpawn, pos, rot, parent);
                    instantiatedArrow = arrowGameObject.GetComponentInChildren<Arrow>();
                    audioSource.PlayOneShot(pullingSound);
                }

                if (instantiatedArrow == null) {
                    return;
                }

                var touchPosition = Input.mousePosition;
                Vector3 pullPosition = camera.ScreenToViewportPoint(touchPosition);
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

            if (Input.GetMouseButtonUp(0)) {

                if (instantiatedArrow == null) {
                    return;
                }

                instantiatedArrow.transform.parent = null;
                var direction = instantiatedArrow.transform.forward;
                var velocity = instantiatedArrow.speed * direction * pullAmount;
                instantiatedArrow.Release(velocity, isSplitingMode);
                if (audioSource.isPlaying) {
                    audioSource.Stop();
                }
                audioSource.PlayOneShot(shootSound);

                if (arrowTypeToInstantiate == ArrowType.Portal) {
                    var portalArrowPrefab = arrowResource.arrowPrefabs[arrowTypeToInstantiate];
                    var portalArrow = portalArrowPrefab.GetComponentInChildren<PortalArrow>();
                    portalArrow.isBlue = !portalArrow.isBlue;
                }

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

