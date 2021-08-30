using UnityEngine;

namespace hittable {
    [RequireComponent(typeof(Hittable))]
    public class ConnectedTarget : MonoBehaviour {

        [SerializeField]
        private ConnectedTarget[] targets;

        [SerializeField]
        private float timeBetweenHits;

        private float hitTime;

        private void Start() {
            hitTime = float.MinValue;
        }

        public void ProcessHit() {
            hitTime = Time.time;

            foreach (var target in targets) {
                float deltaTime = hitTime - target.hitTime;

                if (deltaTime > timeBetweenHits) {
                    return;
                }
            }

            foreach (var target in targets) {
                Destroy(target);
            }

            Destroy(this);
        }

        private void OnDestroy() {
            Destroy(GetComponent<Hittable>());
        }
    }
}