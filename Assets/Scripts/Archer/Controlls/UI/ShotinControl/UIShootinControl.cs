using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Archer.ArcherControlls;

namespace Archer.Controlls.UI.ShootingControlls { 
    public class UIShootinControl : MonoBehaviour {
        [SerializeField] private ShootingController shootingController;
        [SerializeField] private RectTransform startPoint;
        [Space]
        [SerializeField] private RectTransform currentPoint;
        [SerializeField] private RectTransform enableRadius;
        [SerializeField] private RectTransform forceBarContainer;
        [SerializeField] private RectTransform forceBarRight;
        [SerializeField] private RectTransform forceBarLeft;
        [Space]
        [SerializeField] private Sprite disableImage;
        [SerializeField] private Sprite enableImage;

        private RectTransform forceBar => shootingController.IsLeft
            ? forceBarLeft
            : forceBarRight;

        private void Start() {
            ResetControllsVisibility();
        }


        private void Update() {
            ResetControllsVisibility();
            var maxForceDist = (Screen.width > Screen.height ? Screen.height : Screen.width) / 2;
            var radius = shootingController.EnableRadius;
            var sqrRadius = radius * radius;
            var initPointOreNull = shootingController.InitialPoint;
            var currPointOreNull = shootingController.CurrecntPoint;
            if (initPointOreNull == null || currPointOreNull == null) {
                TogglePointEnableState(false);
                return;
            }

            var initialPoint = (Vector2)initPointOreNull;
            var currPoint = (Vector2)currPointOreNull;

            startPoint.position = initialPoint;
            startPoint.gameObject.SetActive(true);

            var diametrValue = shootingController.EnableRadius * 2;
            enableRadius.sizeDelta = new Vector2(diametrValue, diametrValue);
            enableRadius.gameObject.SetActive(true);

            currentPoint.position = currPoint;
            currentPoint.gameObject.SetActive(true);

            if (IsInCircle(initialPoint, currPoint, sqrRadius)) {
                TogglePointEnableState(false);
                forceBarContainer.gameObject.SetActive(false);
                return;
            } else {
                TogglePointEnableState(true);
                currentPoint.gameObject.SetActive(false);
                var forceBarPos = GetCircleOnRadiusPoint(initialPoint, currPoint, radius);
                forceBarContainer.position = forceBarPos;
                forceBarContainer.gameObject.SetActive(true);
                
                var currentRotation = shootingController.CurrentAngle;
                var currentForce = shootingController.CurrentForce;
                var forceBarWidth = maxForceDist * currentForce;
                forceBarContainer.rotation = Quaternion.Euler(0, 0, -currentRotation);
                forceBar.sizeDelta = new Vector2(forceBarWidth, 20);
                forceBar.gameObject.SetActive(true);

            }

        }


        private Vector3 GetCircleOnRadiusPoint(Vector3 center, Vector3 point, float radius) {
            var dir = (point - center).normalized;
            return center + dir * radius;


        }

        private bool IsInCircle(Vector2 centerPoint, Vector2 point, float sqrRadius) {
            return (centerPoint - point).sqrMagnitude < sqrRadius;
        }

        private void TogglePointEnableState(bool isEnable) {
            var startPointImage = startPoint.GetComponent<Image>();
            var currPointImage = currentPoint.GetComponent<Image>();
            if (isEnable) {
                startPointImage.sprite = enableImage;
                currPointImage.sprite = enableImage;
            } else {
                startPointImage.sprite = disableImage;
                currPointImage.sprite = disableImage;
            }
        }

        private void ResetControllsVisibility() {
            startPoint.gameObject.SetActive(false);
            currentPoint.gameObject.SetActive(false);
            enableRadius.gameObject.SetActive(false);
            forceBarContainer.gameObject.SetActive(false);
            forceBarLeft.gameObject.SetActive(false);
            forceBarRight.gameObject.SetActive(false);
        }

    }
}
