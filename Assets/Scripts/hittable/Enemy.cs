using UnityEngine;

namespace hittable {
    [RequireComponent(typeof(Hittable))]
    public class Enemy : MonoBehaviour {

        public void ProcessHit() {
            Destroy(this);
        }

        private void OnDestroy() {
            Destroy(GetComponent<Hittable>());
        }
    }
}