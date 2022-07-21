using UnityEngine;

namespace hittable {
    public class FreezzeController : MonoBehaviour{
        [SerializeField]
        private Animator animator;
        private float originalAnimatorSpeed;

        [SerializeField]
        private Material[] freezeMaterials;

        [SerializeField]
        private float freezeDuration;
        private float unfreezeTime;

        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip freezeSound;

        [SerializeField]
        private bool isFreezeFromStart;
        
        private const float FREEZE_MIN = 0f;
        private const float FREEZE_MAX = 1f;
        private readonly int SHADER_FREEZE_FIELD = Shader.PropertyToID("_IceSlider");

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

            var percent = 1 - (unfreezeTime - Time.time) / freezeDuration;
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
            unfreezeTime = Time.time + freezeDuration;

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

        public void UnfreezeDueBurn() {
            isFreezeFromStart = false;
            unfreezeTime = Time.time + freezeDuration / 2;
        }

        private void OnDestroy() {
            for (int i = 0; i < freezeMaterials.Length; i++) {
                freezeMaterials[i].SetFloat(SHADER_FREEZE_FIELD, FREEZE_MIN);
            }
        }
    }
}
