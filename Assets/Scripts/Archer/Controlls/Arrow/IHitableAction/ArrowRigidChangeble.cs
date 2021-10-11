using Archer.Specs.Rigid;
using UnityEngine;

namespace Archer.Controlls.IHitableAction {
    [RequireComponent(typeof(Rigidbody))]
    public class ArrowRigidChangeble : MonoBehaviour, IHitable {
        [SerializeField] private RigidSpec rigidSpec;

        private Rigidbody rigid;

        private void Start() {
            rigid = GetComponent<Rigidbody>();
        }

        public void HitAction() {
            SetParams();
            SetFlags();
            SetConstraints();
        }

        private void SetParams() {
            if (rigidSpec.IsChaneParams) {
                rigid.mass = rigidSpec.Mass;
                rigid.drag = rigidSpec.Drag;
                rigid.angularDrag = rigidSpec.AngularDrag;
            }
        }

        private void SetFlags() {
            if (rigidSpec.IsChangeFlags) {
                rigid.isKinematic = rigidSpec.IsKinematic;
                rigid.useGravity = rigidSpec.UseGravity;
            }
        }

        private void SetConstraints() {
            rigid.constraints = RigidbodyConstraints.None;
            if (rigidSpec.IsChangeConstraints) {
                var constrain = RigidbodyConstraints.None;
                if (rigidSpec.FreezePosX)
                    constrain |= RigidbodyConstraints.FreezePositionX;
                if (rigidSpec.FreezePosY)
                    constrain |= RigidbodyConstraints.FreezePositionY;
                if (rigidSpec.FreezePosZ)
                    constrain |= RigidbodyConstraints.FreezePositionZ;
                if (rigidSpec.FreezeRotX)
                    constrain |= RigidbodyConstraints.FreezeRotationX;
                if (rigidSpec.FreezeRotY)
                    constrain |= RigidbodyConstraints.FreezeRotationY;
                if (rigidSpec.FreezeRotZ)
                    constrain |= RigidbodyConstraints.FreezeRotationZ;
                rigid.constraints = constrain;
            }
        }

        [ContextMenu("DebugHitAction")]
        private void DebugHitAction() {
            HitAction();
        }
    }
}
