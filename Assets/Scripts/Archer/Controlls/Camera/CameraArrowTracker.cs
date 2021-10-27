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
        [SerializeField] private ShootingController shootingController;
        [SerializeField] private float cameraSpeed;
        [SerializeField] private float cameraTrackSpeed;

        private Coroutine pendingRoutine;
        private Vector3 startPos;

        public bool IsCameraReady => transform.position == startPos;

        private void Start() {
            startPos = transform.position;
        }
        public void WaitAndJumpToStart(float wait = 0f) {
            if (pendingRoutine == null) {
                pendingRoutine = StartCoroutine(WaintAndJump(startPos, wait));
            }
        }

        public void TrackObjects(IEnumerable<Vector3> trackablePos) {
            var center = GetCenter(trackablePos);
            var pos = transform.position;
            var dist = cameraTrackSpeed * Time.deltaTime;
            var zPos = pos.z + dist;
            if (zPos > center.z)
                zPos = center.z;
            var newCameraPos = new Vector3(pos.x, pos.y, zPos);
            transform.position = newCameraPos;
            //transform.position = newCameraPos;
        }

        private IEnumerator WaintAndJump(Vector3 pos, float awaitTime) {
            var waitTimer = 0f;
            while (waitTimer < awaitTime) {
                waitTimer += Time.deltaTime;
                yield return Time.deltaTime;
            }

            var dir = (startPos - transform.position).normalized;

            while (true) {
                yield return Time.deltaTime;
                var cameraPos = transform.position;
                var speed = cameraSpeed * Time.deltaTime;
                var newCamPos = cameraPos + dir * speed;
                if (pos.OnLine(cameraPos, newCamPos)) {

                    transform.position = startPos;
                    break;
                }
                transform.position = newCamPos;
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


    }
}
