using UnityEngine;
using DG.Tweening;

namespace tweening {
    public class TweenMove : MonoBehaviour {
        [SerializeField]
        private Vector3 finalPosition;

        [SerializeField]
        private float time;

        private Tween tween;

        private void Start() {
            tween = transform.DOMove(finalPosition, time).SetLoops(-1, LoopType.Yoyo);
        }

        public void StopMoving() {
            tween.Kill();
        }

#if UNITY_EDITOR

        [ContextMenu("GetFinalPosition")]
        private void GetFinalPosition() {
            finalPosition = transform.position;
        }
#endif
    }
}
