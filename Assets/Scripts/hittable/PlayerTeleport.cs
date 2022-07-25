using UnityEngine;
using arrow;
using level;
using player;

namespace hittable {
    public class PlayerTeleport : MonoBehaviour, IHittable {
        
        [SerializeField] private Player player;
        [SerializeField] private LevelController _levelController;

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
        

        private void Start() {
            renderer.enabled = false;
            collider.enabled = false;
            _levelController.OnTargetsDecrease.AddListener(EnableTeleport);
        }

        private void EnableTeleport()
        {
            if (_levelController.PeelTargetsCount() != appearTargetCount)
            {
                return;;
            }
            renderer.enabled = true;
            collider.enabled = true;
            particle.Play();
        }

        public void ProcessHit(Arrow arrow,RaycastHit hit) {
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