using UnityEngine;
using hittable;
using bow;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Serialization;

namespace level {
    public class LevelController : MonoBehaviour {
        [FormerlySerializedAs("bowController")] [SerializeField]
        private Bow _bow;
        [SerializeField]
        private GameObject winMenu;
        [SerializeField]
        private GameObject failMenu;
        [SerializeField]
        private GameObject gameMenu;

        [SerializeField]
        private ParticleSystem winVfx;

        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip[] winSounds;

        [SerializeField]
        private Star[] stars;
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
        [SerializeField]
        private int targetsCount;

        private int starConditionsCompleteCount;

        private float timeSinceStart;

        [SerializeField]
        private float starScaleSpeed = 0.5f;

        public bool isFailed;

        private void Awake() {
            isFailed = false;
            if(countOfArrowsForStar == 0) {
                countOfArrowsForStar = targetsCount + 1;
            }
            starConditionsCompleteCount = 1;
            winMenu.SetActive(false);
        }

        private void Update() {
            if (gameMenu.activeInHierarchy) {
                timeSinceStart += Time.deltaTime;
            }

            if (targetsCount == 0) {

                if (winVfx != null) {
                    Instantiate(winVfx, transform.position, Quaternion.identity);
                }

                foreach (var sound in winSounds) {
                    audioSource.PlayOneShot(sound);
                }

                gameMenu.SetActive(false);
                winMenu.SetActive(true);

                timeText.text = System.Math.Round(timeSinceStart, 2).ToString();
                arrowsCountText.text = _bow.shotsCount.ToString();

                if(_bow.shotsCount <= countOfArrowsForStar) {
                    starConditionsCompleteCount++;
                }

                if(timeSinceStart <= starTime) {
                    starConditionsCompleteCount++;
                }

                for (int i = 0; i < starConditionsCompleteCount; i++) {
                    stars[i].gameObject.SetActive(true);
                    var transform = stars[i].rectTransform;
                    transform.localScale = Vector3.zero;
                    transform.DOScale(new Vector3(1.12f,1.36f,1), starScaleSpeed);
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

                _bow.enabled = false;
                enabled = false;
            }

            if (isFailed) {

                if(failMenu == null || failTimeText == null || failArrowsCountText == null) {
                    return;
                }

                failMenu.SetActive(true);
                failTimeText.text = System.Math.Round(timeSinceStart, 2).ToString();
                failArrowsCountText.text = _bow.shotsCount.ToString();

                _bow.enabled = false;
                gameMenu.SetActive(false);
                enabled = false;
            }
        }

        public void DecreaseEnemyCount() {
            targetsCount--;
        }

        public int PeelTargetsCount() {
            return targetsCount;
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