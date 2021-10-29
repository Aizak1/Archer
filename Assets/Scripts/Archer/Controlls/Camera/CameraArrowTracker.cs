using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Archer.Controlls.ArrowControlls;
using Archer.ArcherControlls;
using Archer.Extension.Vector3Extension;

namespace Archer.Controlls.CameraControll {
    [RequireComponent(typeof(Camera))]
    public class CameraArrowTracker : MonoBehaviour {
        //[SerializeField] private Collider activeZoneCollider;
        [SerializeField] private float activeZoneLenght;
        [SerializeField] private float activeZoneGap;
        [SerializeField] private float cameraSpeed;
        [SerializeField] private float cameraTrackSpeed;

        private float leftBound;
        private float rightBound;
        private float leftLimitBound;
        private float rightLimitBound;

        private Coroutine pendingRoutine;
        private bool isTraking;
       // private Vector3 startPos;

        public bool IsCameraReady => transform.position.z == leftBound;

        private void Start() {
            SetBounds();
        }

        public void QQQ() {
#if UNITY_ANDROID && !UNITY_EDITOR
                MobileControls();
#endif
#if UNITY_EDITOR
            EditorControls();
#endif
        }

        private void EditorControls() {
            if (isTraking)
                return;

            var isPush = Input.GetMouseButton(2);
            var mousePos = Input.mousePosition;
            var pos = transform.position;

            if (isPush) {
                if (pendingRoutine != null) {
                    StopCoroutine(pendingRoutine);
                    pendingRoutine = null;
                }
                var factor = GetAccselerationFactor(mousePos);
                var dist = cameraSpeed * Time.deltaTime * factor;
                var minMax = GetMinMax(leftLimitBound, rightLimitBound);
                var minLimit = minMax.min;
                var maxLimit = minMax.max;
                var zPos = Mathf.Clamp(pos.z + dist, minLimit, maxLimit);
                var newPos = new Vector3(pos.x, pos.y, zPos);
                transform.position = newPos;
                return;
            }

            if (pos.z >= leftLimitBound && pos.z < leftBound) {
                var newZPos = pos.z + cameraSpeed * Time.deltaTime;
                if (newZPos > leftBound)
                    newZPos = leftBound;
                transform.position = new Vector3(pos.x, pos.y, newZPos);
            } else if (pos.z > rightBound && pos.z <= rightLimitBound) {
                var newZPos = pos.z - cameraSpeed * Time.deltaTime;
                if (newZPos < rightBound)
                    newZPos = rightBound;
                transform.position = new Vector3(pos.x, pos.y, newZPos);
            }
        }

        private void MobileControls() {
            if (isTraking)
                return;

            var pos = transform.position;
            var touchCount = Input.touchCount;
            if (touchCount == 2) {
                if (pendingRoutine != null) {
                    StopCoroutine(pendingRoutine);
                    pendingRoutine = null;
                }
                var firstTouch = Input.GetTouch(0);
                var secondTouch = Input.GetTouch(1);
                var center = (firstTouch.position + secondTouch.position) / 2;
                var factor = GetAccselerationFactor(center);
                var dist = cameraSpeed * Time.deltaTime * factor;
                var minMax = GetMinMax(leftLimitBound, rightLimitBound);
                var minLimit = minMax.min;
                var maxLimit = minMax.max;
                var newZPos = Mathf.Clamp(pos.z + dist, minLimit, maxLimit);
                var newPos = new Vector3(pos.x, pos.y, newZPos);
                transform.position = newPos;
                return;
            }

            if (pos.z >= leftLimitBound && pos.z < leftBound) {
                var newZPos = pos.z + cameraSpeed * Time.deltaTime;
                if (newZPos > leftBound)
                    newZPos = leftBound;
                transform.position = new Vector3(pos.x, pos.y, newZPos);
            } else if (pos.z > rightBound && pos.z <= rightLimitBound) {
                var newZPos = pos.z - cameraSpeed * Time.deltaTime;
                if (newZPos < rightBound)
                    newZPos = rightBound;
                transform.position = new Vector3(pos.x, pos.y, newZPos);
            }

        }

        public void WaitAndJumpToStart(float wait = 0f) {
            if (pendingRoutine == null) {
                pendingRoutine = StartCoroutine(WaitAndJump(leftBound, wait));
            }
        }

        public void TrackObjects(IEnumerable<Vector3> trackablePos) {
            var center = GetCenter(trackablePos);
            var minMax = GetMinMax(leftBound, rightBound);
            var minLimit = minMax.min;
            var maxLimit = minMax.max;

            var pos = transform.position;
            var dist = cameraTrackSpeed * Time.deltaTime;
            var zPos = pos.z + dist;
            if (zPos > center.z)
                zPos = center.z;
            zPos = Mathf.Clamp(zPos, minLimit, maxLimit);
            var newCameraPos = new Vector3(pos.x, pos.y, zPos);
            transform.position = newCameraPos;
        }

        private IEnumerator WaitAndJump(float pos, float awaitTime) {
            var waitTimer = 0f;
            while (waitTimer < awaitTime) {
                waitTimer += Time.deltaTime;
                yield return Time.deltaTime;
            }

            var diff = leftBound - transform.position.z;
            var dir = diff/ Math.Abs(diff);

            while (true) {
                yield return Time.deltaTime;
                var cameraPos = transform.position;
                var speed = cameraSpeed * Time.deltaTime;
                var newZPos = cameraPos.z + dir * speed;
                var minMax = GetMinMax(cameraPos.z, newZPos);

                if (pos >= minMax.min && pos <= minMax.max) {

                    transform.position = new Vector3(cameraPos.x, cameraPos.y, leftBound);
                    break;
                }
                transform.position = new Vector3(cameraPos.x, cameraPos.y, newZPos);
            }
            pendingRoutine = null;
            yield return null;
        }

        private Vector3 GetCenter(IEnumerable<Vector3> posEnumerable) {
            var count = 0;
            var posSum = Vector3.zero;

            foreach (var pos in posEnumerable) {
                count++;
                posSum += pos; 
            }

            return new Vector3(posSum.x / count, posSum.y / count, posSum.z / count);
        }

        private float GetAccselerationFactor(Vector2 pos) {
            var factor = (pos.x - Screen.width / 2) / (Screen.width * 0.45f);
            return Mathf.Clamp(factor, -1, 1);
        }

        private (float min, float max) GetMinMax(float first, float second) {
            var minLimit = first > second ? second : first;
            var maxLimit = first < second ? second : first;
            return (minLimit, maxLimit);
        }

        private void SetBounds() {
            leftBound = transform.position.z;
            rightBound = leftBound + activeZoneLenght;
            leftLimitBound = leftBound - activeZoneGap;
            rightLimitBound = rightBound + activeZoneGap;
        }
    }
}
