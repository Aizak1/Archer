using UnityEngine;
using arrow;

namespace hittable {
    [RequireComponent(typeof(Hittable))]
    public class BurnableObject : MonoBehaviour{

        private bool isBurning;

        [SerializeField]
        private float burnTime;
        private float burnStartTime;

        [SerializeField]
        private new MeshRenderer renderer;

        [SerializeField]
        private Material burnMaterial;

        [SerializeField]
        private BurnCotroller burnCotroller;

        private void Awake() {
            burnCotroller.enabled = false;
        }

        private void Update() {
            if (!isBurning) {
                return;
            }

            float time = Time.time;

            if (time >= burnStartTime + burnTime) {
                Destroy(gameObject);
                return;
            }

            float percent = (time - burnStartTime) / burnTime;

            burnCotroller.animationStage = 1 - percent;
        }

        public void ProcessHit(Arrow arrow) {

            if (isBurning) {
                return;
            }

            Destroy(arrow.gameObject);

            burnStartTime = Time.time;
            isBurning = true;

            renderer.material = burnMaterial;

            burnCotroller.enabled = true;
        }
    }
}