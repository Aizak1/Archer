using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Archer.Controlls.Other { 
    public class StartMenuCameraController : MonoBehaviour {
        [SerializeField] private Transform cameraContainerTransform;
        [SerializeField] private float cameraSpeed;
        [SerializeField] private float distToCenter;

        private float rot = 0;

        private void Start() {
            rot = transform.eulerAngles.x;
        }

        private void LateUpdate() {
            rot = ClampAngle(rot + cameraSpeed, -360, 360);
            var centerPos = cameraContainerTransform.position;
            var cameraPos = transform.position;
            transform.position = centerPos + Quaternion.Euler(20
                , rot, 0f) * (distToCenter * Vector3.back);
            transform.LookAt(centerPos);
        }



        private float ClampAngle(float angle, float min, float max) {
            var a = angle;
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }

    }
}
