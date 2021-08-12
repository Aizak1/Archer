using UnityEngine;
using arrow;

namespace hittable {
    public class BurnableObject : MonoBehaviour{

        [SerializeField]
        private bool isBurning;

        [SerializeField]
        private float burnTime;
        private float burnStartTime;

        private new MeshRenderer renderer;

        [SerializeField]
        private Material burnMaterial;

        private const string FIRE_RALETIVE_HEIGHT = "_FireRelativeheight";

        private void Start() {
            renderer = GetComponent<MeshRenderer>();
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

            burnMaterial.SetFloat(FIRE_RALETIVE_HEIGHT, 1 - percent);
        }

        public void ProcessHit(Arrow arrow) {

            if (isBurning) {
                return;
            }

            if (arrow.arrowType != ArrowType.Fire) {
                return;
            }

            Destroy(arrow.gameObject);

            burnStartTime = Time.time;
            isBurning = true;

            burnMaterial.SetFloat(FIRE_RALETIVE_HEIGHT, 1);

            renderer.material = burnMaterial;
        }
    }
}