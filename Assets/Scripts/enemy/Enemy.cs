using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace enemy {
    public class Enemy : MonoBehaviour {
        public void ApplyDamage() {
            Destroy(gameObject);
        }
    }
}

