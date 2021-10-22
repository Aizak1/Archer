using UnityEngine;

namespace Archer.Controlls.IHitableAction {
    public class ArrowJointBrokable : MonoBehaviour, IHitable {
        [SerializeField] private Joint joint;

        public HitableAccessFlag Type => type;
        private HitableAccessFlag type = HitableAccessFlag.brokeJoint;

        public void HitAction() {
            Destroy(joint);
        }
    }
}
