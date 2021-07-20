using UnityEngine;

namespace player {
    public class PlayerTeleport : MonoBehaviour {
        private void OnDestroy() {
            FindObjectOfType<Player>().gameObject.transform.position = transform.position;
        }
    }
}
