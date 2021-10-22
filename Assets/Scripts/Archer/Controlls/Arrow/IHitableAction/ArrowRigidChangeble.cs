using Archer.Specs.Rigid;
using Archer.Extension.Rigid;
using UnityEngine;

namespace Archer.Controlls.IHitableAction {
    [RequireComponent(typeof(Rigidbody))]
    public class ArrowRigidChangeble : MonoBehaviour, IHitable {
        [SerializeField] private RigidSpec rigidSpec;

        public HitableAccessFlag Type => type;
        private HitableAccessFlag type = HitableAccessFlag.changeRigid;
        private Rigidbody rigid;

        private void Start() {
            rigid = GetComponent<Rigidbody>();
        }

        public void HitAction() {
            rigidSpec.ApplyToRigid(rigid);
        }

        [ContextMenu("DebugHitAction")]
        private void DebugHitAction() {
            HitAction();
        }
    }
}
