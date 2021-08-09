using UnityEngine;

namespace hittable {
    public class FreezzeController : MonoBehaviour{
        [SerializeField]
        private Animator animator;
        private float originalAnimatorSpeed;

        [SerializeField]
        private Material[] freezeMaterials;

        [SerializeField]
        private float freezeTime;
        private float unfreezeTime;

        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip freezeSound;

        [SerializeField]
        private bool isFreezeFromStart;

        private const string SHADER_FREEZE_FIELD = "_IceSlider";
        private const float FREEZE_MIN = 0f;
        private const float FREEZE_MAX = 1f;

        private void Start() {
            originalAnimatorSpeed = animator.speed;

            if (freezeMaterials.Length == 0) {
                enabled = false;
                return;
            }

            if (isFreezeFromStart) {
                Freeze();
            } else {
                Unfreeze();
            }
        }

        private void Update() {
            if (animator.speed != 0 || isFreezeFromStart) {
                return;
            }

            var percent = 1 - (unfreezeTime - Time.time) / freezeTime;
            float freezeValue = Mathf.Lerp(FREEZE_MAX, FREEZE_MIN, percent);

            for (int i = 0; i < freezeMaterials.Length; i++) {
                freezeMaterials[i].SetFloat(SHADER_FREEZE_FIELD, freezeValue);
            }

            if (Time.time < unfreezeTime) {
                return;
            }

            Unfreeze();
        }

        public void Freeze() {
            audioSource.PlayOneShot(freezeSound);
            animator.speed = 0;
            unfreezeTime = Time.time + freezeTime;

            for (int i = 0; i < freezeMaterials.Length; i++) {
                freezeMaterials[i].SetFloat(SHADER_FREEZE_FIELD, FREEZE_MAX);
            }
        }

        public void Unfreeze() {
            animator.speed = originalAnimatorSpeed;

            for (int i = 0; i < freezeMaterials.Length; i++) {
                freezeMaterials[i].SetFloat(SHADER_FREEZE_FIELD, FREEZE_MIN);
            }

            isFreezeFromStart = false;
        }

        private void OnDestroy() {
            for (int i = 0; i < freezeMaterials.Length; i++) {
                freezeMaterials[i].SetFloat(SHADER_FREEZE_FIELD, FREEZE_MIN);
            }
        }
    }
}
