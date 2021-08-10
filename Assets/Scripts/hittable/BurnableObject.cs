using UnityEngine;
using arrow;

namespace hittable {
    public class BurnableObject : MonoBehaviour{

        [SerializeField]
        private bool isBurning;

        [SerializeField]
        private float burnTime;

        private float burnStartTime;
        private Material material;

        private void Start() {
            var renderer = GetComponent<MeshRenderer>();
            material = renderer.material;
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

            // use percent for disolve shader;
        }

        public void ProcessHit(Arrow arrow) {

            if (isBurning) {
                return;
            }

            if (arrow.arrowType != ArrowType.Fire) {
                return;
            }

            burnStartTime = Time.time;
            isBurning = true;
        }
    }
}