using UnityEngine;
using arrow;

namespace hittable {
    public interface IHittable {
        public void ProcessHit(Arrow arrow, RaycastHit hit);
    }
}

