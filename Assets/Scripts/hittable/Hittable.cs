using level;
using UnityEngine;
using arrow;

namespace hittable{
    public class Hittable : MonoBehaviour {

        [SerializeField]
        private Target target;
        [SerializeField]
        private ConnectedTarget connectedTarget;
        [SerializeField]
        private MovingTarget movingTarget;
        [SerializeField]
        private BurnableObject burnableObject;
        [SerializeField]
        private FreezableObject freezableObject;
        [SerializeField]
        private Player player;
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

        public void ProcessHit(Arrow arrow, RaycastHit hit) {
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

            if (player) {
                player.ProcessHit(hit);
                return;
            }

            if (playerTeleport) {
                playerTeleport.ProcessHit(arrow);
            }
        }
    }
}
