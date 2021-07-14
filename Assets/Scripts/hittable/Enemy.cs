using arrow;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hittable {
    public class Enemy : MonoBehaviour, IHittable {
        public void ProcessHit(Arrow arrow, RaycastHit hit) {
            Destroy(gameObject);
        }
    }
}

