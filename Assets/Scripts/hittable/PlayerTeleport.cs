using UnityEngine;
using arrow;
using player;

namespace hittable {
    [RequireComponent(typeof(Hittable))]
    public class PlayerTeleport : MonoBehaviour {
        [SerializeField]
        private Hittable hittable;

        [SerializeField]
        private Player player;

        [SerializeField]
        private new Camera camera;

        [SerializeField]
        private int appearTargetCount;

        [SerializeField]
        private Transform newPosTransform;

        [SerializeField]
        private new MeshRenderer renderer;
        [SerializeField]
        private new BoxCollider collider;

        [SerializeField]
        private ParticleSystem particle;

        private bool isVisible = false;

        private void Start() {
            renderer.enabled = false;
            collider.enabled = false;
        }

        private void Update() {
            if (hittable.levelController.PeelTargetsCount() > appearTargetCount) {
                return;
            }

            if (!isVisible) {
                renderer.enabled = true;
                collider.enabled = true;
                particle.Play();
                isVisible = true;
            }
        }

        public void ProcessHit(Arrow arrow) {
            Destroy(arrow.gameObject);
            if (player == null || newPosTransform == null) {
                return;
            }

            var newPosition = newPosTransform.position;
            var delta = newPosition - player.transform.position;
            player.transform.position = newPosition;
            camera.transform.position += delta;
            Destroy(gameObject);
        }
    }
}