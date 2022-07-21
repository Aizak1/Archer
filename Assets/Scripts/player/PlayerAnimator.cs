using System;
using bow;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Rig[] archerRigs;
        [SerializeField] private Animator archerAnimator;
        private readonly int isShootingID = Animator.StringToHash("isShooting");

        [SerializeField] private Bow _bow;

        private void Start()
        {
            foreach (var rig in archerRigs) {
                rig.weight = 0;
            }

            archerAnimator.SetBool(isShootingID, false);
        }

        private void OnEnable()
        {
            _bow.OnStartPull += OnStartPull;
            _bow.OnEndPull += OnEndPull;
        }

        private void OnDisable()
        {
            _bow.OnStartPull -= OnStartPull;
            _bow.OnEndPull -= OnEndPull;
        }

        private void OnStartPull()
        {
            archerAnimator.SetBool(isShootingID, true);

            foreach (var rig in archerRigs) {
                rig.weight = 1;
            }
        }
        

        private void OnEndPull()
        {
            foreach (var rig in archerRigs) {
                rig.weight = 0;
            }

            archerAnimator.SetBool(isShootingID, false);
        }
    }
}