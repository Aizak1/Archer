using UnityEngine;

namespace Archer.Controlls.Oclusion {
    [RequireComponent(typeof(Rigidbody))]
    public class OclusionBreakable : MonoBehaviour {
        [Range(0, 1)]
        [SerializeField] private float breakeRetaliationFactor;
        [SerializeField] private bool isUnbreakable;
        [SerializeField] private float br�akeTreshold;

        private Rigidbody rigid;

        private void Start() {
            rigid = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision) {
            if (isUnbreakable)
                return;
            if (collision.gameObject.TryGetComponent(out OclusionBreakable other)) {
                var otherRelactiveVelocity = collision.relativeVelocity;
                var otherRigid = other.GetComponent<Rigidbody>();
                var otherMass = otherRigid.mass;
                var otherforce = otherMass * otherRelactiveVelocity.magnitude;
                var retaliationForce = -otherMass * otherRelactiveVelocity;
                otherRigid.AddForce(retaliationForce, ForceMode.Impulse);
                if (otherforce > br�akeTreshold) {
                    //DestroyObject();
                }

                /*
                var dir = rigid.velocity.normalized;
                var velocity = rigid.velocity.magnitude;
                var mass = rigid.mass;
                var force = mass * velocity;

                var otherRigid = other.GetComponent<Rigidbody>();
                var otherDir = otherRigid.velocity.normalized;
                var otherVelocity = otherRigid.velocity.magnitude;
                var otherMass = otherRigid.mass;
                var otherforce = otherMass * otherVelocity;

                var dotproduct = Vector3.Dot(dir, otherDir);
                Debug.Log($"force => {force}");
                Debug.Log($"otherforce => {otherforce}");
                Debug.Log($"dotproduct => {dotproduct}");

                if (otherforce > br�akeTreshold) {
                    var retaliationForce = -otherDir * otherforce * breakeRetaliationFactor;
                    otherRigid.AddForce(retaliationForce, ForceMode.Impulse);
                    DestroyObject();
                }
                */

            }
        }


        private void DestroyObject() {
            // TODO add destruction animation and vfx
            Destroy(gameObject);
        }
    }
}
