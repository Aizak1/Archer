using arrow;
using level;
using UnityEngine;

namespace hittable{
    public class Hittable : MonoBehaviour {
        [SerializeField]
        private Enemy enemy;

        [SerializeField]
        private ConnectedTarget connectedTarget;
        [SerializeField]
        private PatrolingEnemy patrolingEnemy;

        private LevelController levelController;

        private void Start() {
            levelController = FindObjectOfType<LevelController>();
            if (levelController == null) {
                Debug.LogError("Lack of Level Controller");
            }
        }

        public void ProcessHit(Arrow arrow, RaycastHit hit) {
            if (enemy) {
                enemy.ProcessHit();
            } else if (connectedTarget) {
                connectedTarget.ProcessHit();
            } else if (patrolingEnemy) {
                patrolingEnemy.ProcessHit(arrow);
            }
        }

        private void OnDestroy() {
            levelController.DecreaseEnemyCount();
        }
    }
}
