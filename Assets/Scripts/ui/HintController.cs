using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace ui {
    public class HintController : MonoBehaviour{
        [SerializeField]
        private Canvas hintCanvas;

        [SerializeField]
        private GameObject hintObject;

        [SerializeField]
        private Transform finalPosition;

        [SerializeField]
        private float finalScale;

        [SerializeField]
        private TrailRenderer trail;

        [SerializeField]
        private float time;

        [HideInInspector]
        public Canvas gameCanvas;

        private Vector3 startPosition;

        private const int LEVEL_1 = 1;
        private const int LEVEL_18 = 18;

        private const float ACCURACY = 0.1f;

        private void OnEnable() {
            if (hintCanvas) {
                hintCanvas.enabled = true;
                startPosition = hintObject.transform.position;
                if (trail) {
                    trail.enabled = true;
                }
            }

            int levelIndex = SceneManager.GetActiveScene().buildIndex;

            var pos = finalPosition.position;

            switch (levelIndex) {
                case LEVEL_1:
                    var tween = hintObject.transform.DOMove(pos, time).SetLoops(100);
                    tween.OnStepComplete(ClearTrailAfterStep);
                    break;
                case LEVEL_18:
                    hintObject.transform.DOMove(pos, time).SetLoops(100, LoopType.Yoyo);
                    break;
            }
        }

        private void Update() {

            if ((hintObject.transform.position - startPosition).magnitude < ACCURACY) {
                if (trail) {
                    trail.enabled = true;
                }
            }

            if (Input.GetMouseButtonDown(0)) {
                if (hintCanvas) {
                    hintCanvas.enabled = false;
                    if (trail) {
                        trail.Clear();
                        trail.enabled = false;
                    }

                }
                enabled = false;

                gameCanvas.enabled = true;

                hintObject.transform.DOKill();
            }
        }

        private void ClearTrailAfterStep() {
            if (trail) {
                trail.Clear();
                trail.enabled = false;
            }
        }
    }
}