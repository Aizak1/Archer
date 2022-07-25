using arrow;
using arrow.behaviours;
using UnityEngine;

namespace hittable {
    public class FreezableObject : MonoBehaviour {

        [SerializeField]
        private FreezzeController freezzeController;

        public void ProcessHit(Arrow arrow,RaycastHit hit) {
            foreach (var item in arrow.AdditionalArrowBehaviors)
            {
                if (item.BehaviorType == BehaviorType.Freeze) {
                    freezzeController.Freeze();
                    return;
                }
                
                if (item.BehaviorType == BehaviorType.Fire) {
                    freezzeController.UnfreezeDueBurn();
                    return;
                }
            }
            
        }
    }
}