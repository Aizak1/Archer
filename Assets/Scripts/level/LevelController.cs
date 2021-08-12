using UnityEngine;
using hittable;
using bow;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace level {
    public class LevelController : MonoBehaviour {
        [SerializeField]
        private BowController bowController;
        [SerializeField]
        private Canvas winCanvas;
        [SerializeField]
        private Canvas failCanvas;
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
        private TextMeshProUGUI failTimeText;
        [SerializeField]
        private TextMeshProUGUI failArrowsCountText;

        [SerializeField]
        private float starTime;
        [SerializeField]
        private int countOfArrowsForStar;
        private int enemiesCount;


        private int starConditionsCompleteCount;

        private float timeSinceStart;

        [SerializeField]
        private float starScaleSpeed = 0.5f;

        public bool isFailed;

        private void Awake() {
            enemiesCount = FindObjectsOfType<Hittable>().Length;
            isFailed = false;
            if(countOfArrowsForStar == 0) {
                countOfArrowsForStar = enemiesCount + 1;
            }
            starConditionsCompleteCount = 1;
            winCanvas.enabled = false;
        }

        private void Update() {
            if (bowController.enabled) {
                timeSinceStart += Time.deltaTime;
            }

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
                arrowsCountText.text = bowController.shotsCount.ToString();

                if(bowController.shotsCount <= countOfArrowsForStar) {
                    starConditionsCompleteCount++;
                }

                if(timeSinceStart <= starTime) {
                    starConditionsCompleteCount++;
                }

                for (int i = 0; i < starConditionsCompleteCount; i++) {
                    stars[i].SetActive(true);
                    var transform = stars[i].GetComponent<RectTransform>();
                    transform.localScale = Vector3.zero;
                    transform.DOScale(Vector3.one, starScaleSpeed);
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
                        int index = nextLevelIndex - 2;
                        var value = starConditionsCompleteCount.ToString();
                        var newValue = starsAtLevels.Remove(index,1).Insert(index,value);
                        PlayerPrefs.SetString(LevelsManager.STARTS_AT_LEVELS, newValue);
                    }
                }

                bowController.enabled = false;
                enabled = false;
            }

            if (isFailed) {

                if(failCanvas == null || failTimeText == null || failArrowsCountText == null) {
                    return;
                }

                failCanvas.enabled = true;
                failTimeText.text = System.Math.Round(timeSinceStart, 2).ToString();
                failArrowsCountText.text = bowController.shotsCount.ToString();

                bowController.enabled = false;
                gameCanvas.enabled = false;
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

