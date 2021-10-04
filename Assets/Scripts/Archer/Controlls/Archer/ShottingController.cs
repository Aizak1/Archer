using UnityEngine;
using Archer.Specs.Bow;
using Archer.Controlls.ArrowControlls;

namespace Archer.ArcherControlls {
    public class ShottingController : MonoBehaviour {
        [SerializeField] private ArcherAnimatorController archerAnimatorController;
        [SerializeField] private Transform arrowPool;
        [SerializeField] private GameObject arrowPlacementPoint;
        [SerializeField] private GameObject bowRotationPivot;

        [SerializeField] private BowSpec bowSpec;

        [SerializeField] private GameObject arrowPrefab;

        private ArrowController arrowController;

        private float targetAngle;
        private float targetForce;
        private float force;

        private Vector3? initialPoint;
        private Vector3? currecntPoint;
        private bool isAmmoLoaded;
        private bool isLeft;

        private void Start() {
            archerAnimatorController.SetRigsValues(0);
            archerAnimatorController.SetShotting(false);
        }

        private void Update() {
            EditorControls();
            /*
            #if UNITY_EDITOR
                EditorControls();
            #endif
            #if UNITY_ANDROID
            #endif
            */
            //MobileControls();
        }

        private void MobileControls() {

            var touchCount = Input.touchCount;
            if (touchCount > 0) {
                var zeroTouch = Input.GetTouch(0);
                var prevPos = currecntPoint;
                currecntPoint = zeroTouch.position;
                if (zeroTouch.phase == TouchPhase.Began && arrowController != null
                    && arrowController.IsInAir) {
                    arrowController.Split();
                    return;
                }

                UpdateAngle(targetAngle);
                UpdateForce(targetForce);

                if (arrowController != null && !arrowController.WasShoot
                    && initialPoint != null && zeroTouch.phase == TouchPhase.Ended) {
                    initialPoint = null;
                    archerAnimatorController.SetShotForce(0);
                    arrowController.transform.SetParent(arrowPool);
                    arrowController.Release(force * bowSpec.MaxForce);
                    ResetAngleAndForce();
                    bowRotationPivot.transform.rotation = Quaternion.Euler(0, 0, 0);
                    //SHOOT
                    isAmmoLoaded = false;
                    archerAnimatorController.SetShotting(false);
                    archerAnimatorController.SetRigsValues(0);
                }

                if (initialPoint != null && currecntPoint != null && prevPos != null) {
                    var delta = (Vector3)prevPos - (Vector3)currecntPoint;
                    var deltaMagnitude = delta.magnitude;
                    if (deltaMagnitude < 2) {
                        return;
                    }
                }

                if (zeroTouch.phase == TouchPhase.Began) {
                    initialPoint = currecntPoint;
                    return;
                }


                if (zeroTouch.phase == TouchPhase.Moved && initialPoint != null) {
                    var inCircle = IsInCircle(
                        (Vector3)initialPoint, (Vector3)currecntPoint, bowSpec.InitialRadius);
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
                            (Vector2)initialPoint, (Vector2)currecntPoint) - bowSpec.InitialRadius;
                        var maxD = (Screen.width > Screen.height ? Screen.height : Screen.width) / 2;
                        var forcePercent = distFromInit / maxD;
                        targetForce = forcePercent;
                    }
                }
            }
        }

        private void EditorControls() {
            var isMouseDown = Input.GetMouseButtonDown(0);
            var isMouseUp = Input.GetMouseButtonUp(0);
            var isMousePressed = Input.GetMouseButton(0);

            var prevPos = currecntPoint;
            currecntPoint = Input.mousePosition;

            if (isMouseDown && arrowController != null && arrowController.IsInAir) {
                arrowController.Split();
            }

            UpdateAngle(targetAngle);
            UpdateForce(targetForce);

            if (arrowController != null && !arrowController.WasShoot
                && initialPoint != null && isMouseUp) {
                initialPoint = null;
                archerAnimatorController.SetShotForce(0);
                arrowController.transform.SetParent(arrowPool);
                arrowController.Release(force * bowSpec.MaxForce);
                ResetAngleAndForce();
                bowRotationPivot.transform.rotation = Quaternion.Euler(0, 0, 0);
                //SHOOT
                isAmmoLoaded = false;
                archerAnimatorController.SetShotting(false);
                archerAnimatorController.SetRigsValues(0);
                return;
            }

            if (initialPoint != null && currecntPoint != null && prevPos != null) {
                var delta = (Vector3)prevPos - (Vector3)currecntPoint;
                var deltaMagnitude = delta.magnitude;
                if (deltaMagnitude < 2) {
                    return;
                }
            }

            if (isMouseDown) {
                initialPoint = currecntPoint;
            }

            if (isMousePressed && initialPoint != null) {
                var inCircle = IsInCircle(
                    (Vector3)initialPoint, (Vector3)currecntPoint, bowSpec.InitialRadius);
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
                    } else
                        if (localCurrentPoint.x < localInitPoint.x)
                        currecntPoint = new Vector3(
                            localInitPoint.x + 5, localCurrentPoint.y, 0);

                    var direction = ((Vector3)currecntPoint - (Vector3)initialPoint).normalized;
                    var hupotinuse =
                        Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2));
                    var sin = direction.y / hupotinuse;
                    var angle = Mathf.Asin(sin) * Mathf.Rad2Deg;
                    if (!isLeft)
                        angle *= -1;
                    targetAngle = angle;

                    var distFromInit = Vector2.Distance(
                        (Vector2)initialPoint, (Vector2)currecntPoint) - bowSpec.InitialRadius;
                    var maxD = (Screen.width > Screen.height ? Screen.height : Screen.width) / 2;
                    var forcePercent = distFromInit / maxD;
                    targetForce = forcePercent;
                }
            }

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
    }
}
