using UnityEngine;
using arrow;
using level;
using player;

namespace hittable {
    public class PlayerTeleport : MonoBehaviour, IHittable {
        
        [SerializeField] private Player _player;
        [SerializeField] private LevelController _levelController;
        
        [SerializeField] private int _appearTargetCount;
        [SerializeField] private Transform _newPosTransform;
        
        [SerializeField]private ParticleSystem _particle;
        
        private new Camera _camera;
        private new MeshRenderer _renderer;
        private new BoxCollider _collider;

        private void Start() {
            
            _camera = Camera.main;
            _renderer = GetComponent<MeshRenderer>();
            _collider = GetComponent<BoxCollider>();
            
            _renderer.enabled = false;
            _collider.enabled = false;
            _levelController.OnTargetsDecrease.AddListener(EnableTeleport);
        }

        private void EnableTeleport()
        {
            if (_levelController.TargetsCount != _appearTargetCount)
            {
                return;;
            }
            _renderer.enabled = true;
            _collider.enabled = true;
            _particle.Play();
        }

        public void ProcessHit(Arrow arrow,RaycastHit hit) {
            Destroy(arrow.gameObject);
            if (_player == null || _newPosTransform == null) {
                return;
            }

            var newPosition = _newPosTransform.position;
            var delta = newPosition - _player.transform.position;
            _player.transform.position = newPosition;
            _camera.transform.position += delta;
            Destroy(gameObject);
        }
    }
}