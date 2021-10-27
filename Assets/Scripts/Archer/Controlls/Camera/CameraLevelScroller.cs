using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Archer.Controlls.CameraControll {
    [RequireComponent(typeof(Camera))]
    public class CameraLevelScroller : MonoBehaviour {
        [SerializeField] private Vector3 startPos;
        [SerializeField] private Vector3 endPos;

        [SerializeField] private Vector3 startLimitPos;
        [SerializeField] private Vector3 endLimitPos;

        [SerializeField] private float speed;

        private CameraArrowTracker cameraArrowTracker;

        private void Update() {
            var isPush = Input.GetMouseButton(2);
            var mousePos = Input.mousePosition;
            var pos = transform.position;
            if (isPush) {
                var factor = GetAccselerationFactor(mousePos);
                var dist = speed * Time.deltaTime * factor;
                var minLimit = startLimitPos.z > endLimitPos.z ? endLimitPos.z : startLimitPos.z;
                var maxLimit = startLimitPos.z < endLimitPos.z ? endLimitPos.z : startLimitPos.z;
                var zPos = Mathf.Clamp(pos.z + dist, minLimit, maxLimit);
                var newPos = new Vector3(pos.x, pos.y, zPos);
                transform.position = newPos;
                return;
            }

            if (pos != startPos || pos != endPos) {
                var zPos = pos.z;
                if(zPos >= startLimitPos.z && zPos < startPos.z) {
                    var newZPos = zPos + speed * Time.deltaTime;
                    if (newZPos > startPos.z)
                        newZPos = startPos.z;
                    transform.position = new Vector3(pos.x, pos.y, newZPos);
                } else if (zPos > endPos.z && zPos <= endLimitPos.z) {
                    var newZPos = zPos - speed * Time.deltaTime;
                    if (newZPos < endPos.z)
                        newZPos = endPos.z;
                    transform.position = new Vector3(pos.x, pos.y, newZPos);
                }
            }
        }

        private float GetAccselerationFactor(Vector2 pos) {
            var factor = (pos.x - Screen.width / 2) / (Screen.width * 0.45f);
            return Mathf.Clamp(factor, -1, 1);
        }


        [ContextMenu("SetStartPosition")]
        private void SetStartPosition() {
            startPos = transform.position;
        }

        [ContextMenu("SetEndPosition")]
        private void SetEndPosition() {
            endPos = transform.position;
        }

        [ContextMenu("SetStartLimitPosition")]
        private void SetStartLimitPosition() {
            startLimitPos = transform.position;
        }

        [ContextMenu("SetEndLimitPosition")]
        private void SetEndLimitPosition() {
            endLimitPos = transform.position;
        }
    }
}