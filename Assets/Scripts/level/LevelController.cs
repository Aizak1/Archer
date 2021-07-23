using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hittable;

namespace level {
    public class LevelController : MonoBehaviour {
        [SerializeField]
        private SceneLoader sceneLoader;

        private int enemiesCount;

        private void Awake() {
            enemiesCount = FindObjectsOfType<Enemy>().Length;
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
    }
}

