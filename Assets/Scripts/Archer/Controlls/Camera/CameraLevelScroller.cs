using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archer.Controlls.CameraControll {
    [RequireComponent(typeof(Camera))]
    public class CameraLevelScroller : MonoBehaviour {
        [SerializeField] private Vector3 leftBound;
        [SerializeField] private Vector3 rightBound;

        [SerializeField] private Vector3 leftLimitBound;
        [SerializeField] private Vector3 rightLimitBound;

        [SerializeField] private float speed;

        private CameraArrowTracker cameraArrowTracker;

        private void Update() {
#if UNITY_ANDROID
            MobileControls();
#endif
#if UNITY_EDITOR
            EditorControls();
#endif
        }

        private void EditorControls() {
            var isPush = Input.GetMouseButton(2);
            var mousePos = Input.mousePosition;
            var pos = transform.position;
            if (isPush) {
                var factor = GetAccselerationFactor(mousePos);
                var dist = speed * Time.deltaTime * factor;
                var minLimit = leftLimitBound.z > rightLimitBound.z ? rightLimitBound.z : leftLimitBound.z;
                var maxLimit = leftLimitBound.z < rightLimitBound.z ? rightLimitBound.z : leftLimitBound.z;
                var zPos = Mathf.Clamp(pos.z + dist, minLimit, maxLimit);
                var newPos = new Vector3(pos.x, pos.y, zPos);
                transform.position = newPos;
                return;
            }

            if (pos != leftBound || pos != rightBound) {
                var zPos = pos.z;
                if (zPos >= leftLimitBound.z && zPos < leftBound.z) {
                    var newZPos = zPos + speed * Time.deltaTime;
                    if (newZPos > leftBound.z)
                        newZPos = leftBound.z;
                    transform.position = new Vector3(pos.x, pos.y, newZPos);
                } else if (zPos > rightBound.z && zPos <= rightLimitBound.z) {
                    var newZPos = zPos - speed * Time.deltaTime;
                    if (newZPos < rightBound.z)
                        newZPos = rightBound.z;
                    transform.position = new Vector3(pos.x, pos.y, newZPos);
                }
            }
        }

        private void MobileControls() {
            var pos = transform.position;
            var touchCount = Input.touchCount;
            if (touchCount == 2) {
                var firstTouch = Input.GetTouch(0);
                var secondTouch = Input.GetTouch(1);
                var center = (firstTouch.position + secondTouch.position) / 2;
                var factor = GetAccselerationFactor(center);
                var dist = speed * Time.deltaTime * factor;
                var minLimit = leftLimitBound.z > rightLimitBound.z ? rightLimitBound.z : leftLimitBound.z;
                var maxLimit = leftLimitBound.z < rightLimitBound.z ? rightLimitBound.z : leftLimitBound.z;
                var newZPos = Mathf.Clamp(pos.z + dist, minLimit, maxLimit);
                var newPos = new Vector3(pos.x, pos.y, newZPos);
                transform.position = newPos;
                return;
            }

            var zPos = pos.z;
            if (zPos >= leftLimitBound.z && zPos < leftBound.z) {
                var newZPos = zPos + speed * Time.deltaTime;
                if (newZPos > leftBound.z)
                    newZPos = leftBound.z;
                transform.position = new Vector3(pos.x, pos.y, newZPos);
            } else if (zPos > rightBound.z && zPos <= rightLimitBound.z) {
                var newZPos = zPos - speed * Time.deltaTime;
                if (newZPos < rightBound.z)
                    newZPos = rightBound.z;
                transform.position = new Vector3(pos.x, pos.y, newZPos);
            }




        }

        private float GetAccselerationFactor(Vector2 pos) {
            var factor = (pos.x - Screen.width / 2) / (Screen.width * 0.45f);
            return Mathf.Clamp(factor, -1, 1);
        }

        [ContextMenu("SetStartPosition")]
        private void SetStartPosition() {
            leftBound = transform.position;
        }

        [ContextMenu("SetEndPosition")]
        private void SetEndPosition() {
            rightBound = transform.position;
        }

        [ContextMenu("SetStartLimitPosition")]
        private void SetStartLimitPosition() {
            leftLimitBound = transform.position;
        }

        [ContextMenu("SetEndLimitPosition")]
        private void SetEndLimitPosition() {
            rightLimitBound = transform.position;
        }
    }
}