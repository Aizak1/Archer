using UnityEngine;

namespace Archer.Controlls.IHitableAction {
    public class ArrowObjectDestroyeble : MonoBehaviour, IHitable {

        public HitableAccessFlag Type => type;
        private HitableAccessFlag type = HitableAccessFlag.destroyObject;

        public void HitAction() {
            Destroy(gameObject);
        }
    }
}
