using UnityEngine;

namespace Archer.Controlls.IHitableAction {
    public class ArrowCurruptedChangeState : MonoBehaviour, IHitable {
        [SerializeField] private ArrowObjectCorruptable objectCorruptable;
        public HitableAccessFlag Type => type;
        private HitableAccessFlag type = HitableAccessFlag.corrupt;

        public void HitAction() {
            objectCorruptable.SetChangeState();
        }
    }

}
