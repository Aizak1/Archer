using UnityEngine;
using hittable;
using bow;
using TMPro;
using UnityEngine.SceneManagement;

namespace level {
    public class LevelController : MonoBehaviour {
        [SerializeField]
        private BowController bowController;
        [SerializeField]
        private Canvas winCanvas;
        [SerializeField]
        private Canvas gameCanvas;

        [SerializeField]
        private ParticleSystem winVfx;

        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip[] winSounds;

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
            Time.timeScale = 1;
        }

        private void Update() {
            timeSinceStart += Time.deltaTime;

            if (enemiesCount == 0) {

                if (winVfx != null) {
                    Instantiate(winVfx, transform.position, Quaternion.identity);
                }

                foreach (var sound in winSounds) {
                    audioSource.PlayOneShot(sound);
                }

                gameCanvas.enabled = false;
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

                var nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
                if (nextLevelIndex == SceneManager.sceneCountInBuildSettings) {
                    nextLevelIndex = 0;
                }
                if (nextLevelIndex > PlayerPrefs.GetInt(LevelsManager.LEVEL_AT)) {
                    PlayerPrefs.SetInt(LevelsManager.LEVEL_AT, nextLevelIndex);
                }

                var starsAtLevels = PlayerPrefs.GetString(LevelsManager.STARTS_AT_LEVELS);

                if (nextLevelIndex - 1 > starsAtLevels.Length) {
                    var value = starsAtLevels + $"{starConditionsCompleteCount}";
                    PlayerPrefs.SetString(LevelsManager.STARTS_AT_LEVELS,value);
                } else {
                    char starsAtLevel = starsAtLevels[nextLevelIndex - 2];
                    if (starConditionsCompleteCount > int.Parse(starsAtLevel.ToString())) {
                        var newValue = starsAtLevels.Remove(nextLevelIndex - 2,1).Insert(nextLevelIndex - 2,starConditionsCompleteCount.ToString());
                        PlayerPrefs.SetString(LevelsManager.STARTS_AT_LEVELS, newValue);
                    }
                }

                bowController.enabled = false;
                enabled = false;
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

