using UnityEngine;
using hittable;
using bow;
using TMPro;

namespace level {
    public class LevelController : MonoBehaviour {
        [SerializeField]
        private BowController bowController;
        [SerializeField]
        private Canvas winCanvas;

        [SerializeField]
        private ParticleSystem winVfx;

        [SerializeField]
        private GameObject[] stars;
        [SerializeField]
        private TextMeshProUGUI timeText;
        [SerializeField]
        private TextMeshProUGUI arrowsCountText;

        [SerializeField]
        private float starTime;

        private int enemiesCount;
        private int countOfArrowsForStar;

        private int starConditionsCompleteCount;

        private float timeSinceStart;


        private void Awake() {
            enemiesCount = FindObjectsOfType<Hittable>().Length;
            countOfArrowsForStar = enemiesCount + 1;
            starConditionsCompleteCount = 1;
            winCanvas.enabled = false;
        }

        private void Update() {
            timeSinceStart += Time.deltaTime;
            if (enemiesCount == 0) {

                if(winVfx != null) {
                    Instantiate(winVfx, transform.position, Quaternion.identity);
                }

                winCanvas.enabled = true;
                timeText.text = System.Math.Round(timeSinceStart, 2).ToString();
                arrowsCountText.text = bowController.arrowsWasted.ToString();

                if(bowController.arrowsWasted <= countOfArrowsForStar) {
                    starConditionsCompleteCount++;
                }

                if(timeSinceStart <= starTime) {
                    starConditionsCompleteCount++;
                }

                for (int i = 0; i < starConditionsCompleteCount; i++) {
                    stars[i].SetActive(true);
                }

                bowController.enabled = false;
                enabled = false;
                return;
            }
        }

        public void DecreaseEnemyCount() {
            enemiesCount--;
        }

        public int PeelEnemiesCount() {
            return enemiesCount;
        }
        public int PeelCountOfArrowsForStar() {
            return countOfArrowsForStar;
        }

        public float PeelStarTime() {
            return starTime;
        }

        public float PeelTimeSinceStart() {
            return timeSinceStart;
        }
    }
}

