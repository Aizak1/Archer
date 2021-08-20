using UnityEngine;

namespace bow {
    public class BowAnimator : MonoBehaviour {
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private float maxPull;

        private const string BLEND = "Blend";

        public void UpdatePull(in float pullAmount) {
            animator.SetFloat(BLEND, pullAmount * maxPull);
        }
    }
}