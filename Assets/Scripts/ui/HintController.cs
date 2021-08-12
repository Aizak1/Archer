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
        private float time;

        [HideInInspector]
        public Canvas gameCanvas;

        private const int LEVEL_1 = 1;
        private const int LEVEL_18 = 18;

        private void OnEnable() {
            if (hintCanvas) {
                hintCanvas.enabled = true;
            }

            int levelIndex = SceneManager.GetActiveScene().buildIndex;

            var pos = finalPosition.position;

            switch (levelIndex) {
                case LEVEL_1:
                    hintObject.transform.DOMove(pos, time).SetLoops(100);
                    break;
                case LEVEL_18:
                    hintObject.transform.DOMove(pos, time).SetLoops(100, LoopType.Yoyo);
                    break;
            }
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                if (hintCanvas) {
                    hintCanvas.enabled = false;
                }
                enabled = false;

                gameCanvas.enabled = true;

                hintObject.transform.DOKill();
            }
        }
    }
}