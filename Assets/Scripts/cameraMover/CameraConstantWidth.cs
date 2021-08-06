using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cameraMover {
    public class CameraConstantWidth : MonoBehaviour {

        [SerializeField]
        private Vector2 resolution = new Vector2(1920,1080);
        [SerializeField]
        [Range(0f, 1f)]
        private float widthOrHeight = 0.5f;

        private Camera componentCamera;

        private float initialSize;
        private float targetAspect;

        private float initialFov;
        private float horizontalFov = 120f;

        private void Start() {
            componentCamera = GetComponent<Camera>();
            initialSize = componentCamera.orthographicSize;

            targetAspect = resolution.x / resolution.y;

            initialFov = componentCamera.fieldOfView;
            horizontalFov = CalcVerticalFov(initialFov, 1 / targetAspect);
        }

        private void Update() {
            if (componentCamera.orthographic) {
                float constantWidthSize = initialSize * (targetAspect / componentCamera.aspect);
                var size = Mathf.Lerp(constantWidthSize, initialSize, widthOrHeight);
                componentCamera.orthographicSize = size;
            } else {
                float constantWidthFov = CalcVerticalFov(horizontalFov, componentCamera.aspect);
                var fieldOfView = Mathf.Lerp(constantWidthFov, initialFov, widthOrHeight);
                componentCamera.fieldOfView = fieldOfView;
            }
        }

        private float CalcVerticalFov(float hFovInDeg, float aspectRatio) {
            float hFovInRads = hFovInDeg * Mathf.Deg2Rad;

            float vFovInRads = 2 * Mathf.Atan(Mathf.Tan(hFovInRads / 2) / aspectRatio);

            return vFovInRads * Mathf.Rad2Deg;
        }
    }
}

