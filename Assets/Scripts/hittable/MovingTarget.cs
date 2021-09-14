using UnityEngine;
using tweening;

namespace hittable {
    [RequireComponent(typeof(Hittable))]
    [RequireComponent(typeof(TweenMove))]
    public class MovingTarget : MonoBehaviour {
        [SerializeField]
        private TweenMove tweenMove;

        public void ProcessHit() {
            tweenMove.StopMoving();
            Destroy(this);
        }
    }
}
