using UnityEngine;
using hittable;
using arrow;

namespace player {
    public class PlayerTeleport : MonoBehaviour {

        [SerializeField]
        private Hittable[] hittablesToDestroy;

        [SerializeField]
        private Transform newPosTransform;

        private new MeshRenderer renderer;
        private new BoxCollider collider;

        private void Start() {
            renderer = GetComponent<MeshRenderer>();
            collider = GetComponent<BoxCollider>();

            renderer.enabled = false;
            collider.enabled = false;
        }


        private void Update() {
            for (int i = 0; i < hittablesToDestroy.Length; i++) {
                if(hittablesToDestroy[i] != null) {
                    return;
                }
            }
            renderer.enabled = true;
            collider.enabled = true;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.GetComponent<Arrow>()) {
                Destroy(other.gameObject);
                var player = FindObjectOfType<Player>();
                if (player == null || newPosTransform == null) {
                    return;
                }
                var delta = newPosTransform.position - player.gameObject.transform.position;
                player.gameObject.transform.position = newPosTransform.position;
                Camera.main.transform.position += delta;
                Destroy(gameObject);
            }
        }
    }
}
