using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Archer.ArcherControlls;

namespace Archer.Controlls.UI.ShootingControlls { 
    public class UIShootinControl : MonoBehaviour {
        [SerializeField] private ShottingController shootingController;
        [SerializeField] private RectTransform startPoint;
        [SerializeField] private RectTransform currentPoint;
        [SerializeField] private RectTransform enableRadius;

        [SerializeField] private Sprite disableImage;
        [SerializeField] private Sprite enableImage;

        private void Start() {
            ToggleControllsVisibility(false);
        }


        private void Update() {
            var initPointOreNull = shootingController.InitialPoint;
            if (initPointOreNull == null) {
                ToggleControllsVisibility(false);
                return;
            }

            var diametrValue = shootingController.EnableRadius * 2;
            enableRadius.sizeDelta = new Vector2(diametrValue, diametrValue);
            enableRadius.gameObject.SetActive(true);





            var initialPoint = (Vector2)initPointOreNull;
            startPoint.position = initialPoint;
            startPoint.gameObject.SetActive(true);
            var currPointOreNull = shootingController.CurrecntPoint;
            if (currPointOreNull == null) {
                ToggleControllsVisibility(false);
                return;
            }
            var currPoint = (Vector2)currPointOreNull;
            currentPoint.position = currPoint;
            currentPoint.gameObject.SetActive(true);
            //Debug.Log(shootingController.IsAmmoLoaded);


        }


        private bool IsInCircle(Vector2 centerPoint, Vector2 point, float radius) {
            var sqrRadius = radius * radius;
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

        private void ToggleControllsVisibility(bool isVisible) {
            startPoint.gameObject.SetActive(false);
            currentPoint.gameObject.SetActive(false);
            enableRadius.gameObject.SetActive(false);
            if (isVisible) {
                startPoint.gameObject.SetActive(true);
                currentPoint.gameObject.SetActive(true);
                enableRadius.gameObject.SetActive(true);
            }
        }

    }
}
