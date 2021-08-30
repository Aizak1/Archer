using level;
using UnityEngine;

namespace hittable{
    public class Hittable : MonoBehaviour {

        [SerializeField]
        private Target target;
        [SerializeField]
        private ConnectedTarget connectedTarget;
        [SerializeField]
        private MovingTarget movingTarget;

        private LevelController levelController;

        private void Start() {
            levelController = FindObjectOfType<LevelController>();
            if (levelController == null) {
                Debug.LogError("Lack of Level Controller");
            }
        }

        public void ProcessHit() {
            if (target) {
                target.ProcessHit();
                return;
            }

            if (connectedTarget) {
                connectedTarget.ProcessHit();
                return;
            }

            if (movingTarget) {
                movingTarget.ProcessHit();
                return;
            }
        }

        private void OnDestroy() {
            levelController.DecreaseEnemyCount();
        }
    }
}
