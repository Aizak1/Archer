using UnityEngine;
using arrow;

namespace ricochet {
    public class RicochetSurface : MonoBehaviour {

        public void Richochet(Arrow arrow) {
            var rigidBody = arrow.GetComponent<Rigidbody>();
            var velocity = rigidBody.velocity;
            var newVelocity = new Vector3(velocity.x, -velocity.y, velocity.z);
            rigidBody.velocity = newVelocity;
        }
    }
}