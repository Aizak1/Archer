using arrow;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ui;
using portal;

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

        private Vector3 startTouchPosition;

        [HideInInspector]
        public float pullAmount;
        [HideInInspector]
        public ArrowType arrowTypeToInstantiate;
        [HideInInspector]
        public Arrow instantiatedArrow;

        [SerializeField]
        private Rig[] archerRigs;
        [SerializeField]
        private Animator archerAnimator;
        private readonly int isShootingID = Animator.StringToHash("isShooting");

        [SerializeField]
        private new Camera camera;

        [HideInInspector]
        public int shotsCount;
        public const int MAX_ARROWS_COUNT = 13;
        [HideInInspector]
        public Queue<GameObject> arrowsOnLevel;

        [SerializeField]
        private Animator bowAnimator;
        [SerializeField]
        private float maxPull;
        private readonly int blendID = Animator.StringToHash("Blend");

        [SerializeField]
        private TrajectoryShower trajectoryShower;

        [SerializeField]
        public PortalController portalController;

        private void Start() {
            shotsCount = 0;
            arrowsOnLevel = new Queue<GameObject>();
            arrowTypeToInstantiate = arrowResource.countToArrowType[0];

            foreach (var rig in archerRigs) {
                rig.weight = 0;
            }

            archerAnimator.SetBool(isShootingID, false);
        }

        private void Update() {

            if (Input.GetMouseButtonDown(0)) {

                var uiObject = EventSystem.current.currentSelectedGameObject;

                if (uiObject && uiObject.GetComponent<Button>()) {
                    return;
                }

                startTouchPosition = camera.ScreenToViewportPoint(Input.mousePosition);
                startTouchPosition.z = maxPullTransform.position.z;

                var pos = arrowPlacementPoint.transform.position;
                var rot = arrowPlacementPoint.transform.rotation;
                var parent = arrowPlacementPoint.transform;

                var arrowObjectToSpawn = arrowResource.arrowPrefabs[arrowTypeToInstantiate];
                var arrowGameObject = Instantiate(arrowObjectToSpawn, pos, rot, parent);
                instantiatedArrow = arrowGameObject.GetComponentInChildren<Arrow>();
                instantiatedArrow.enabled = false;

                audioSource.PlayOneShot(pullingSound);

                archerAnimator.SetBool(isShootingID, true);

                foreach (var rig in archerRigs) {
                    rig.weight = 1;
                }

                if (trajectoryShower) {
                    trajectoryShower.StartDraw();
                }
            }

            if (instantiatedArrow == null) {
                return;
            }

            if (Input.GetMouseButton(0)) {

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

                bowAnimator.SetFloat(blendID, pullAmount * maxPull);

                if (trajectoryShower) {
                    trajectoryShower.Draw();
                }
            }

            if (Input.GetMouseButtonUp(0)) {

                instantiatedArrow.enabled = true;

                instantiatedArrow.transform.parent = null;

                var x = bowRotationPivot.transform.parent.transform.position.x;
                var y = instantiatedArrow.transform.position.y;
                var z = instantiatedArrow.transform.position.z;

                instantiatedArrow.transform.position = new Vector3(x, y, z);

                var rot = instantiatedArrow.transform.rotation;
                rot.y = 0;
                rot.z = 0;
                instantiatedArrow.transform.rotation = rot;

                var direction = instantiatedArrow.transform.forward;
                var velocity = instantiatedArrow.speed * pullAmount * direction;
                instantiatedArrow.Release(velocity, true, this);

                if (audioSource.isPlaying) {
                    audioSource.Stop();
                }
                audioSource.PlayOneShot(shootSound);
                shotsCount++;

                if (arrowTypeToInstantiate == ArrowType.Portal) {
                    portalController.isBlue = !portalController.isBlue;
                }

                pullAmount = 0;
                instantiatedArrow = null;

                foreach (var rig in archerRigs) {
                    rig.weight = 0;
                }

                archerAnimator.SetBool(isShootingID, false);

                bowAnimator.SetFloat(blendID, pullAmount * maxPull);

                if (trajectoryShower) {
                    trajectoryShower.EndDraw();
                }
            }
        }

        private float CalculatePullAmount(Vector3 pullPosition) {
            var pullVector = pullPosition - startTouchPosition;
            var maxPullVector = maxPullTransform.position - minPullTransform.position;

            float pullAmount = pullVector.magnitude / maxPullVector.magnitude;

            return Mathf.Clamp(pullAmount, 0, 1);
        }
    }
}