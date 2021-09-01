using UnityEngine;

namespace cameraMover {
    public class CameraConstantWidth : MonoBehaviour {

        [SerializeField]
        private Vector2 resolution = new Vector2(1920,1080);
        [SerializeField]
        [Range(0f, 1f)]
        private float widthOrHeight = 0.5f;

        [SerializeField]
        private new Camera camera;

        private float initialSize;
        private float targetAspect;

        private float initialFov;
        private float horizontalFov = 120f;

        private void Start() {
            initialSize = camera.orthographicSize;

            targetAspect = resolution.x / resolution.y;

            initialFov = camera.fieldOfView;
            horizontalFov = CalcVerticalFov(initialFov, 1 / targetAspect);

            if (camera.orthographic) {
                float constantWidthSize = initialSize * (targetAspect / camera.aspect);
                var size = Mathf.Lerp(constantWidthSize, initialSize, widthOrHeight);
                camera.orthographicSize = size;
            } else {
                float constantWidthFov = CalcVerticalFov(horizontalFov, camera.aspect);
                var fieldOfView = Mathf.Lerp(constantWidthFov, initialFov, widthOrHeight);
                camera.fieldOfView = fieldOfView;
            }
        }

        private float CalcVerticalFov(float hFovInDeg, float aspectRatio) {
            float hFovInRads = hFovInDeg * Mathf.Deg2Rad;

            float vFovInRads = 2 * Mathf.Atan(Mathf.Tan(hFovInRads / 2) / aspectRatio);

            return vFovInRads * Mathf.Rad2Deg;
        }
    }
}