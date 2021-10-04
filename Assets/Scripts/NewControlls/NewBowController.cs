using arrow;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ui;
using portal;

namespace bow {
    public class NewBowController : MonoBehaviour {
        [SerializeField] private Transform minPullTransform;
        [SerializeField] private Transform maxPullTransform;
        [SerializeField] private GameObject arrowPlacementPoint;
        [SerializeField] private GameObject bowRotationPivot;
        [SerializeField] private ArrowResource arrowResource;

        /*
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip shootSound;
        [SerializeField] private AudioClip pullingSound;
        */

        [SerializeField] private Rig[] archerRigs;
        [SerializeField] private Animator archerAnimator;
        [SerializeField] private Animator bowAnimator;
        [SerializeField] private new Camera camera;
        [SerializeField] private TrajectoryShower trajectoryShower;
        [SerializeField] public PortalController portalController;
        [SerializeField] private BowSpec bowSpec;

        [HideInInspector] public ArrowType arrowTypeToInstantiate;
        [HideInInspector] public Queue<GameObject> arrowsOnLevel;

        private readonly int isShootingID = Animator.StringToHash("isShooting");
        private readonly int blendID = Animator.StringToHash("Blend");

        [HideInInspector] public Arrow instantiatedArrow;
        private GameObject operationArrow;

        [SerializeField] private float initialRadius;
        private Vector3? initialPoint;
        private Vector3? currecntPoint;
        private bool isAmmoLoaded;
        private bool isLeft;

        private float force;

        private void Start() {
            arrowsOnLevel = new Queue<GameObject>();
            arrowTypeToInstantiate = arrowResource.countToArrowType[0];

            foreach (var rig in archerRigs) {
                rig.weight = 0;
            }

            archerAnimator.SetBool(isShootingID, false);
        }

        private void Update() {
            var isMouseDown = Input.GetMouseButtonDown(0);
            var isMouseUp = Input.GetMouseButtonUp(0);
            var isMousePressed = Input.GetMouseButton(0);

            var targetForce = 0f;
            var targetAngle = 0f;

            currecntPoint = Input.mousePosition;

            if (isMouseUp) {
                initialPoint = null;
                //ShootAction?.Invoke();
                isAmmoLoaded = false;
                return;
            }

            if (isMouseDown) {
                initialPoint = currecntPoint;
            }

            if (isMousePressed && initialPoint != null) {
                var inCircle = IsInCircle(
                    (Vector3)initialPoint, (Vector3)currecntPoint, initialRadius);
                if (isAmmoLoaded && inCircle) {
                    isAmmoLoaded = false;
                } else if (!isAmmoLoaded && !inCircle) {
                    isAmmoLoaded = true;
                    var xDiff = ((Vector3)initialPoint).x - ((Vector3)currecntPoint).x;
                    if (xDiff < 0)
                        isLeft = false;
                    else
                        isLeft = true;
                }

                LoadArrow(isAmmoLoaded);

                if (isAmmoLoaded) {
                    var localCurrentPoint = (Vector3)currecntPoint;
                    var localInitPoint = (Vector3)initialPoint;
                    if (isLeft) {
                        if (localCurrentPoint.x > localInitPoint.x)
                            currecntPoint = new Vector3(
                                localInitPoint.x - 5, localCurrentPoint.y, 0);
                    } else
                        if (localCurrentPoint.x < localInitPoint.x)
                        currecntPoint = new Vector3(
                            localInitPoint.x + 5, localCurrentPoint.y, 0);

                    var direction = ((Vector3)currecntPoint - (Vector3)initialPoint).normalized;
                    var hupotinuse =
                        Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2));
                    var sin = direction.y / hupotinuse;
                    var angle = Mathf.Asin(sin) * Mathf.Rad2Deg;
                    if (isLeft)
                        angle *= -1;
                    targetAngle = angle;
                    Debug.Log($"targetAngle => {targetAngle}");

                    var distFromInit = Vector2.Distance(
                        (Vector2)initialPoint, (Vector2)currecntPoint) - initialRadius;
                    var maxD = (Screen.width > Screen.height ? Screen.height : Screen.width) / 2;
                    var forcePercent = distFromInit / maxD;
                    targetForce = forcePercent;

                    UpdateAngleAndForce(targetForce, targetAngle);

                    //bowAnimator.SetFloat(blendID, force);

                    /*
                    if (trajectoryShower) {
                        trajectoryShower.Draw();
                    }
                    */
                }
            }


        }

        public void UpdateAngleAndForce(float targetForce, float targetAngle) {
            //var currentAngle = bowRotationPivot.transform.rotation.eulerAngles.z;
            var currentAngle = transform.rotation.eulerAngles.z;
            
            currentAngle = FormatAngle(currentAngle);
            var directionMultiplyer = -Mathf.Sign(currentAngle - targetAngle);
            var specAnglePreFrame = bowSpec.AngleChange * Time.deltaTime;
            var newAngle = specAnglePreFrame * directionMultiplyer + currentAngle;

            transform.rotation = Quaternion.AngleAxis(newAngle, Vector3.left);

            var clampTargetForce = Mathf.Clamp01(targetForce);
            var targetForcePowerLimit = clampTargetForce * bowSpec.MaxForce;
            var currecntForce = force;
            var forceDirection = Mathf.Sign(targetForcePowerLimit - currecntForce);

            if (forceDirection == -1) {
                force -= bowSpec.ForveDecrement * Time.deltaTime;
                if (force < targetForcePowerLimit)
                    force = targetForcePowerLimit;
                if (force < 0)
                    force = 0;
            } else {
                force += bowSpec.ForceIncrement * Time.deltaTime;
                if (force > targetForcePowerLimit)
                    force = targetForcePowerLimit;
                if (force > bowSpec.MaxForce)
                    force = bowSpec.MaxForce;
            }
        }

        private void LoadArrow(bool isArrowLoaded) {
            if (isArrowLoaded) {
                var pos = arrowPlacementPoint.transform.position;
                var rot = arrowPlacementPoint.transform.rotation;
                var parent = arrowPlacementPoint.transform;

                var arrowObjectToSpawn = arrowResource.arrowPrefabs[arrowTypeToInstantiate];
                operationArrow = Instantiate(arrowObjectToSpawn, pos, rot, parent);
                instantiatedArrow = operationArrow.GetComponentInChildren<Arrow>();
                instantiatedArrow.enabled = false;

                //audioSource.PlayOneShot(pullingSound);

                archerAnimator.SetBool(isShootingID, true);

                foreach (var rig in archerRigs) {
                    rig.weight = 1;
                }
                /*
                if (trajectoryShower) {
                    trajectoryShower.StartDraw();
                }
                */
            } else {
                //audioSource.PlayOneShot(pullingSound);
                Destroy(operationArrow);
                instantiatedArrow = null;
                archerAnimator.SetBool(isShootingID, false);

                foreach (var rig in archerRigs) {
                    rig.weight = 1;
                }
            }
        }

        private void HandleTouchInput() {
            var isMouseDown = Input.GetMouseButtonDown(0);
            var isMouseUp = Input.GetMouseButtonUp(0);
            var isMousePressed = Input.GetMouseButton(0);

            var targetForce = 0f;
            var targetAngle = 0f;

            currecntPoint = Input.mousePosition;

            if (isMouseUp) {
                initialPoint = null;
                //ShootAction?.Invoke();
                isAmmoLoaded = false;
                return;
            }

            if (isMouseDown) {
                initialPoint = currecntPoint;
            }

            if (isMousePressed && initialPoint != null) {
                var inCircle = IsInCircle((Vector3)initialPoint, (Vector3)currecntPoint, initialRadius);
                if (isAmmoLoaded && inCircle) {
                    isAmmoLoaded = false;
                }
                if (!isAmmoLoaded && !inCircle) {
                    isAmmoLoaded = true;
                    var xDiff = ((Vector3)initialPoint).x - ((Vector3)currecntPoint).x;
                    if (xDiff < 0)
                        isLeft = false;
                    else
                        isLeft = true;
                }
                if (isAmmoLoaded) {
                    var localCurrentPoint = (Vector3)currecntPoint;
                    var localInitPoint = (Vector3)initialPoint;
                    if (isLeft) {
                        if (localCurrentPoint.x > localInitPoint.x)
                            currecntPoint = new Vector3(localInitPoint.x - 5, localCurrentPoint.y, 0);
                    } else
                        if (localCurrentPoint.x < localInitPoint.x)
                        currecntPoint = new Vector3(localInitPoint.x + 5, localCurrentPoint.y, 0);

                    var direction = ((Vector3)currecntPoint - (Vector3)initialPoint).normalized;
                    var hupotinuse = Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2));
                    var sin = direction.y / hupotinuse;
                    var angle = Mathf.Asin(sin) * Mathf.Rad2Deg;
                    if (isLeft)
                        angle *= -1;
                    targetAngle = angle;


                    var distFromInit = Vector2.Distance((Vector2)initialPoint, (Vector2)currecntPoint);
                    var maxDist = Screen.width / 2;
                    var forcePercent = distFromInit / maxDist;
                    targetForce = forcePercent;
                }
            }

            //bowController.UpdateAngleAndForce(isAmmoLoaded, targetForce, targetAngle);
        }

        private bool IsInCircle(Vector3 center, Vector3 point, float radius) {
            return Vector2.Distance(center, point) < radius / 2;
        }

        private float FormatAngle(float angle) {
            var localAngle = angle;
            if (angle > 0 && angle < 180) {

            } else if (angle > 180 && angle < 360) {
                localAngle = -(360 - angle);
            }
            return localAngle;
        }

    }

    [Serializable]
    public struct BowSpec {
        public float MaxForce;
        public float ForceIncrement;
        public float ForveDecrement;
        public float AngleChange;
        public float InitialRadius;

        public BowSpec(
            float maxForce,
            float forceIncrement,
            float forveDecrement,
            float angleChange,
            float initialRadius) {
            MaxForce = maxForce;
            ForceIncrement = forceIncrement;
            ForveDecrement = forveDecrement;
            AngleChange = angleChange;
            InitialRadius = initialRadius;
        }
    }
}
