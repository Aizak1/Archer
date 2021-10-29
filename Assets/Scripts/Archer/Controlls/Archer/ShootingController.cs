using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Archer.Specs.Bow;
using Archer.Controlls.ArrowControlls;
using Archer.Controlls.CameraControll;
using Archer.Extension.List;

namespace Archer.ArcherControlls {
    public class ShootingController : MonoBehaviour {
        [SerializeField] private CameraArrowTracker cameraArrowTracker;
        [SerializeField] private ArcherAnimatorController archerAnimatorController;
        [SerializeField] private Transform arrowPool;
        [SerializeField] private GameObject arrowPlacementPoint;
        [SerializeField] private GameObject bowRotationPivot;
        [SerializeField] private BowSpec bowSpec;
        [SerializeField] private float enableRadius;
        [SerializeField] private GameObject arrowPrefab;
        public bool IsAmmoLoaded => isAmmoLoaded;
        public Vector3? InitialPoint => initialPoint;
        public Vector3? CurrecntPoint => currecntPoint;
        public bool IsLeft => isLeft;
        public float EnableRadius => enableRadius;
        public float CurrentForce => force;
        public float CurrentAngle =>
            bowRotationPivot.transform.rotation.eulerAngles.x;

        private bool isOuterControl;
        private ArrowController arrowController;
        private List<ArrowController> splitArrowsList;

        private float targetAngle;
        private float targetForce;
        private float force;

        private Vector3? initialPoint;
        private Vector3? currecntPoint;
        private bool isAmmoLoaded;
        private bool isLeft;

        private bool IsArrowTraking;

        private void Start() {
            archerAnimatorController.SetRigsValues(0);
            archerAnimatorController.SetShotting(false);
            splitArrowsList = new List<ArrowController>();
            if (TryGetComponent(out ShootingControllerSimulator _))
                isOuterControl = true;
        }

        private void Update() {
            if (!isOuterControl) {
#if UNITY_ANDROID && !UNITY_EDITOR
                MobileControls();
#endif
#if UNITY_EDITOR
                EditorControls();
#endif
            }
        }

        private void LateUpdate() {
            if (arrowController == null && splitArrowsList.Count == 0)
                IsArrowTraking = false;

            if (IsArrowTraking)
                cameraArrowTracker.TrackObjects(TryGetTracableArrowPos());
            else
                cameraArrowTracker.QQQ();
        }

        private void OnTriggerExit(Collider collider) {
            if (collider.gameObject.TryGetComponent(out ArrowController _)) {
                IsArrowTraking = true;
            }
        }

        private void MobileControls() {
            if (splitArrowsList.Count > 0)
                splitArrowsList.UpdateAvalibility();

            var touchCount = Input.touchCount;
            if (touchCount != 1) {
                initialPoint = null;
                if (isAmmoLoaded) {
                    isAmmoLoaded = false;
                    LoadArrow(isAmmoLoaded);
                }
                return;
            }

            var touch = Input.GetTouch(0);
            var touchPos = touch.position;
            var sqtRadius = enableRadius * enableRadius;
            var isNoArrow = arrowController == null && splitArrowsList.Count == 0;
            currecntPoint = touchPos;

            if (touch.phase == TouchPhase.Ended
                && arrowController != null && arrowController.IsInAir) {
                arrowController.TryToSplit(out var splitArrows);
                splitArrowsList = splitArrows.ToList();
                return;
            }


            UpdateAngle(targetAngle);
            UpdateForce(targetForce);

            if (arrowController != null && !arrowController.WasShoot
                && initialPoint != null && touch.phase == TouchPhase.Ended) {
                initialPoint = null;
                archerAnimatorController.SetShotForce(0);
                arrowController.SetArrowPool(arrowPool);
                arrowController.Release(force * bowSpec.MaxForce);
                ResetAngleAndForce();
                bowRotationPivot.transform.rotation = Quaternion.Euler(0, 0, 0);
                isAmmoLoaded = false;
                archerAnimatorController.SetShotting(false);
                archerAnimatorController.SetRigsValues(0);
                return;
            }

            if (touch.phase == TouchPhase.Ended && initialPoint != null
                && arrowController == null) {
                initialPoint = null;
                currecntPoint = null;
                if (isAmmoLoaded) {
                    isAmmoLoaded = false;
                    LoadArrow(isAmmoLoaded);
                }
            }

            if (initialPoint != null && touch.deltaPosition.sqrMagnitude < 4) {
                return;
            }

            if (touch.phase == TouchPhase.Began && isNoArrow) {
                if (cameraArrowTracker.IsCameraReady)
                    initialPoint = currecntPoint;
                else
                    cameraArrowTracker.WaitAndJumpToStart(0);
            }



            if (touch.phase == TouchPhase.Moved && initialPoint != null) {
                var inCircle = IsInCircle(
                    (Vector3)initialPoint, (Vector3)currecntPoint, sqtRadius);
                if (isAmmoLoaded && inCircle) {
                    isAmmoLoaded = false;
                    LoadArrow(isAmmoLoaded);
                } else if (!isAmmoLoaded && !inCircle) {
                    isAmmoLoaded = true;
                    LoadArrow(isAmmoLoaded);
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
                            currecntPoint = new Vector3(
                                localInitPoint.x - 5, localCurrentPoint.y, 0);
                    } else {
                        if (localCurrentPoint.x < localInitPoint.x)
                            currecntPoint = new Vector3(
                                localInitPoint.x + 5, localCurrentPoint.y, 0);
                    }

                    var direction = ((Vector3)currecntPoint - (Vector3)initialPoint).normalized;
                    var hupotinuse =
                        Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2));
                    var sin = direction.y / hupotinuse;
                    var angle = Mathf.Asin(sin) * Mathf.Rad2Deg;
                    if (!isLeft)
                        angle *= -1;
                    targetAngle = angle;

                    var distFromInit = Vector2.Distance(
                        (Vector2)initialPoint, (Vector2)currecntPoint) - enableRadius;
                    var maxD = (Screen.width > Screen.height ? Screen.height : Screen.width) / 2;
                    var forcePercent = distFromInit / maxD;
                    targetForce = forcePercent;
                }
            }




        }

        private void EditorControls() {
            if (splitArrowsList.Count > 0)
                splitArrowsList.UpdateAvalibility();
            var sqtRadius = enableRadius * enableRadius;
            var isMouseDown = Input.GetMouseButtonDown(0);
            var isMouseUp = Input.GetMouseButtonUp(0);
            var isMousePressed = Input.GetMouseButton(0);
            var isNoArrow = arrowController == null && splitArrowsList.Count == 0;
            var prevPos = currecntPoint;
            currecntPoint = Input.mousePosition;

            if (isMouseDown && arrowController != null && arrowController.IsInAir) {
                arrowController.TryToSplit(out var splitArrows);
                splitArrowsList = splitArrows.ToList();
                return;
            }

            UpdateAngle(targetAngle);
            UpdateForce(targetForce);

            if (arrowController != null && !arrowController.WasShoot
                && initialPoint != null && isMouseUp) {
                initialPoint = null;
                archerAnimatorController.SetShotForce(0);
                arrowController.SetArrowPool(arrowPool);
                arrowController.Release(force * bowSpec.MaxForce);
                ResetAngleAndForce();
                bowRotationPivot.transform.rotation = Quaternion.Euler(0, 0, 0);
                isAmmoLoaded = false;
                archerAnimatorController.SetShotting(false);
                archerAnimatorController.SetRigsValues(0);
                return;
            }


            if (isMouseUp && initialPoint != null && arrowController == null) {
                initialPoint = null;
                currecntPoint = null;
                if (isAmmoLoaded) {
                    isAmmoLoaded = false;
                    LoadArrow(isAmmoLoaded);
                }
            }

            if (initialPoint != null && currecntPoint != null && prevPos != null) {
                var sqrtDelta = ((Vector3)prevPos - (Vector3)currecntPoint).sqrMagnitude;
                if (sqrtDelta < 4) {
                    return;
                }
            }


            if (isMouseDown && isNoArrow) {
                if (cameraArrowTracker.IsCameraReady)
                    initialPoint = currecntPoint;
                else
                    cameraArrowTracker.WaitAndJumpToStart(0);
            }

            if (isMousePressed && initialPoint != null) {
                var inCircle = IsInCircle(
                    (Vector3)initialPoint, (Vector3)currecntPoint, sqtRadius);
                if (isAmmoLoaded && inCircle) {
                    isAmmoLoaded = false;
                    LoadArrow(isAmmoLoaded);
                } else if (!isAmmoLoaded && !inCircle) {
                    isAmmoLoaded = true;
                    LoadArrow(isAmmoLoaded);
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
                            currecntPoint = new Vector3(
                                localInitPoint.x - 5, localCurrentPoint.y, 0);
                    } else {
                        if (localCurrentPoint.x < localInitPoint.x)
                        currecntPoint = new Vector3(
                            localInitPoint.x + 5, localCurrentPoint.y, 0);
                    }

                    var direction = ((Vector3)currecntPoint - (Vector3)initialPoint).normalized;
                    var hupotinuse =
                        Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2));
                    var sin = direction.y / hupotinuse;
                    var angle = Mathf.Asin(sin) * Mathf.Rad2Deg;
                    if (!isLeft)
                        angle *= -1;
                    targetAngle = angle;

                    var distFromInit = Vector2.Distance(
                        (Vector2)initialPoint, (Vector2)currecntPoint) - enableRadius;
                    var maxD = (Screen.width > Screen.height ? Screen.height : Screen.width) / 2;
                    var forcePercent = distFromInit / maxD;
                    targetForce = forcePercent;
                }
            }

        }

        private bool IsInCircle(Vector3 center, Vector3 point, float sqtRadius) {
            return (center - point).sqrMagnitude < sqtRadius;
        }

        private float FormatAngle(float angle) {
            var localAngle = angle;
            if (angle > 0 && angle < 180) {

            } else if (angle > 180 && angle < 360) {
                localAngle = -(360 - angle);
            }
            return localAngle;
        }

        private void LoadArrow(bool isArrowLoaded) {
            ResetAngleAndForce();
            if (isArrowLoaded) {
                var pos = arrowPlacementPoint.transform.position;
                var rot = arrowPlacementPoint.transform.rotation;
                var parent = arrowPlacementPoint.transform;

                var operationArrow = Instantiate(arrowPrefab, pos, rot, parent);
                arrowController = operationArrow.GetComponent<ArrowController>();
                arrowController.enabled = false;
                archerAnimatorController.SetShotting(true);
                archerAnimatorController.SetRigsValues(1);
            } else {
                if (arrowController != null)
                    Destroy(arrowController.gameObject);
                archerAnimatorController.SetShotting(false);
                archerAnimatorController.SetRigsValues(0);
            }
        }

        private void ResetAngleAndForce() {
            targetAngle = 0;
            targetForce = 0;
            force = 0;
            bowRotationPivot.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        public void UpdateAngle(float targetAngle) {
            var currentAngle = bowRotationPivot.transform.rotation.eulerAngles.x;
            currentAngle = FormatAngle(currentAngle);
            var directionMultiplyer = -Mathf.Sign(currentAngle - targetAngle);
            var specAnglePreFrame = bowSpec.AngleChange * Time.deltaTime;
            var deltaIncrement = specAnglePreFrame * directionMultiplyer;
            var newAngle = currentAngle + deltaIncrement;
            if (Mathf.Abs(targetAngle - currentAngle) < specAnglePreFrame) {
                newAngle = targetAngle;
            }

            bowRotationPivot.transform.rotation = Quaternion.Euler(newAngle, 0, 0);
        }

        public void UpdateForce(float targetForce) {
            var clampTarget = Mathf.Clamp01(targetForce);
            var targetForcePowerLimit = clampTarget * bowSpec.MaxForce;
            var currecntForce = force * bowSpec.MaxForce;
            var forceDirection = Mathf.Sign(targetForcePowerLimit - currecntForce);
            var localForce = currecntForce;
            if (forceDirection == -1) {
                localForce -= bowSpec.ForveDecrement * Time.deltaTime;
                if (localForce < targetForcePowerLimit)
                    localForce = targetForcePowerLimit;
                if (localForce < 0)
                    localForce = 0;
            } else {
                localForce += bowSpec.ForceIncrement * Time.deltaTime;
                if (localForce > targetForcePowerLimit)
                    localForce = targetForcePowerLimit;
                if (localForce > bowSpec.MaxForce)
                    localForce = bowSpec.MaxForce;
            }

            force = localForce / bowSpec.MaxForce;

            var k = force; // 0,1
            var kSecond = Mathf.Lerp(0, 0.6f, k); // 0 .6
            archerAnimatorController.SetShotForce(kSecond);
        }

        public void OuterControl(float targetAngle, float targetForce, bool arrowRelese) {
            if (!isAmmoLoaded) {
                isAmmoLoaded = true;
                LoadArrow(true);
                return;
            }
            if (arrowRelese) {
                archerAnimatorController.SetShotForce(0);
                arrowController.SetArrowPool(arrowPool);
                arrowController.Release(force * bowSpec.MaxForce);
                ResetAngleAndForce();
                bowRotationPivot.transform.rotation = Quaternion.Euler(0, 0, 0);
                isAmmoLoaded = false;
                archerAnimatorController.SetShotting(false);
                archerAnimatorController.SetRigsValues(0);
                return;
            }
            this.targetAngle = targetAngle;
            this.targetForce = targetForce;

            UpdateAngle(targetAngle);
            UpdateForce(targetForce);

        }

        public Vector3[] TryGetTracableArrowPos() {
            var posList = new List<Vector3>();
            if (arrowController != null)
                posList.Add(arrowController.transform.position);
            if (splitArrowsList.Count > 0) {
                foreach (var arrow in splitArrowsList) {
                    posList.Add(arrow.transform.position);
                }
            }
            return posList.ToArray();
        }
    }
}
