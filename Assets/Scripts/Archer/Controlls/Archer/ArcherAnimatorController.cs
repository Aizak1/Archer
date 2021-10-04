using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Archer.ArcherControlls {
    public class ArcherAnimatorController : MonoBehaviour {
        [SerializeField] private Animator archerAnimator;
        [SerializeField] private Animator bowAnimator;
        [SerializeField] private Rig[] archerRigs;

        private readonly int isShootingID = Animator.StringToHash("isShooting");
        private readonly int blendID = Animator.StringToHash("Blend");

        public void SetRigsValues(float value) {
            foreach (var rig in archerRigs) {
                rig.weight = value;
            }
        }

        public void SetShotting(bool isShotting) {
            archerAnimator.SetBool(isShootingID, isShotting);
        }

        public void SetShotForce(float force) {
            bowAnimator.SetFloat(blendID, force);
        }
    }
}
