using UnityEngine;

namespace bow {
    public class BowAnimator : MonoBehaviour {
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private BowController bowController;

        private const string BLEND = "Blend";

        private void Update() {
            animator.SetFloat(BLEND, bowController.pullAmount);
        }
    }
}

