using UnityEngine;

namespace Archer.Controlls.IHitableAction {
    public class ArrowJointBrokable : MonoBehaviour, IHitable {
        [SerializeField] private Joint joint;

        public void HitAction() {
            Destroy(joint);
        }
    }
}
