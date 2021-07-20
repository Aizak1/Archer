using arrow;
using UnityEngine;

namespace hittable {
    public class Enemy : MonoBehaviour, IHittable {
        public void ProcessHit(Arrow arrow, RaycastHit hit) {
            Destroy(gameObject);
        }
    }
}

