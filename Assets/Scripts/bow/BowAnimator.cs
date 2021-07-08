using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bow {
    public class BowAnimator : MonoBehaviour {
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private BowController puller;

        private const string BLEND = "Blend";

        private void Update() {
            animator.SetFloat(BLEND, puller.pullAmount);
        }
    }
}

