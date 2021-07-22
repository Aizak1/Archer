using arrow;
using level;
using UnityEngine;

namespace hittable {
    public class Enemy : MonoBehaviour, IHittable {
        private LevelController levelController;

        private void Start() {
            levelController = FindObjectOfType<LevelController>();
            if(levelController == null) {
                Debug.LogError("Lack of Level Controller");
            }
        }

        public void ProcessHit(Arrow arrow, RaycastHit hit) {
            Destroy(gameObject);
        }

        private void OnDestroy() {
            levelController.DecreaseEnemyCount();
        }
    }
}

