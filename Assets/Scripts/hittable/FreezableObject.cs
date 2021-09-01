using arrow;
using UnityEngine;

namespace hittable {
    [RequireComponent(typeof(Hittable))]
    public class FreezableObject : MonoBehaviour {

        [SerializeField]
        private FreezzeController freezzeController;

        public void ProcessHit(Arrow arrow) {
            if (arrow.arrowType == ArrowType.Freeze) {
                freezzeController.Freeze();
                return;
            }

            if (arrow.arrowType == ArrowType.Fire) {
                freezzeController.UnfreezeDueBurn();
                return;
            }
        }
    }
}