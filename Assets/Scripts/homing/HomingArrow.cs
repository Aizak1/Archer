using arrow;
using UnityEngine;

namespace homing {
    public class HomingArrow : MonoBehaviour {

        public HomingArrowPoint homingArrowPoint;
        private Transform targetForHomingArrow;

        public Transform currentPoint;

        public const float SPEED = 10;

        private void Awake() {
            homingArrowPoint = FindObjectOfType<HomingArrowPoint>();
            var targets = FindObjectsOfType<HomingArrowTarget>();
            if (targets.Length == 0) {
                GetComponent<Arrow>().arrowType = ArrowType.Wooden;
                return;
            }
            var target = targets[Random.Range(0, targets.Length)];
            targetForHomingArrow = target.transform;
            Destroy(target);

            currentPoint = homingArrowPoint.transform;
        }

        public void ChangeCurrentPoint() {
            currentPoint = targetForHomingArrow.transform;
        }
    }
}

