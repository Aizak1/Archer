using UnityEngine;

namespace hittable {
    [RequireComponent(typeof(Hittable))]
    public class SimpleTarget : MonoBehaviour {
        public void ProcessHit() {
            Destroy(this);
        }
    }
}