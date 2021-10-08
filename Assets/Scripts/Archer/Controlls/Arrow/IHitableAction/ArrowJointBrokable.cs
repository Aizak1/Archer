using UnityEngine;

namespace Archer.Controlls.IHitableAction {
    public class ArrowJointBrokable : MonoBehaviour, IHitable {
        private Joint joint;

        private void Start()
        {
            joint = GetComponent<Joint>();
        }

        public void HitAction()
        {

        }
    }
}
