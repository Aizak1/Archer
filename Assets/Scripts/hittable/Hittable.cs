using level;
using UnityEngine;
using arrow;

namespace hittable{
    public class Hittable : MonoBehaviour {

        [SerializeField]
        private SimpleTarget target;
        [SerializeField]
        private ConnectedTarget connectedTarget;
        [SerializeField]
        private MovingTarget movingTarget;
        [SerializeField]
        private BurnableObject burnableObject;
        [SerializeField]
        private FreezableObject freezableObject;
        [SerializeField]
        private PlayerTeleport playerTeleport;

        [SerializeField]
        public LevelController levelController;

        private void Start() {
            if (levelController == null) {
                Debug.LogError("Lack of Level Controller");
                enabled = false;
            }
        }

        public void ProcessHit(Arrow arrow) {
            if (target) {
                target.ProcessHit();
                levelController.DecreaseEnemyCount();
                return;
            }

            if (connectedTarget) {
                connectedTarget.ProcessHit();
                return;
            }

            if (movingTarget) {
                movingTarget.ProcessHit();
                levelController.DecreaseEnemyCount();
                return;
            }

            if (burnableObject) {
                burnableObject.ProcessHit(arrow);
                return;
            }

            if (freezableObject) {
                freezableObject.ProcessHit(arrow);
                return;
            }
            
            if (playerTeleport) {
                playerTeleport.ProcessHit(arrow);
            }
        }
    }
}
