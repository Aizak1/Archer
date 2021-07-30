using UnityEngine;
using hittable;

namespace level {
    public class LevelController : MonoBehaviour {
        [SerializeField]
        private SceneLoader sceneLoader;
        [SerializeField]
        private float starTime;

        private int enemiesCount;
        private int countOfArrowsForStar;


        private void Awake() {
            enemiesCount = FindObjectsOfType<Hittable>().Length;
            countOfArrowsForStar = enemiesCount - 1;
        }

        private void Update() {
            if (enemiesCount == 0) {
                sceneLoader.LoadNextLevel();
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
    }
}

