using UnityEngine;

namespace player {
    public class PlayerTeleport : MonoBehaviour {
        private void OnDestroy() {
            var player = FindObjectOfType<Player>();
            if (player == null) {
                return;
            }
            player.gameObject.transform.position = transform.position;
        }
    }
}
