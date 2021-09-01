using UnityEngine;
using level;

namespace hittable {
    [RequireComponent(typeof(Hittable))]
    public class Player : MonoBehaviour {
        [SerializeField]
        private LevelController levelController;
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip auchSound;
        [SerializeField]
        private ParticleSystem blood;

        public void ProcessHit(RaycastHit hit) {

            if (levelController == null) {
                return;
            }

            levelController.isFailed = true;
            if (blood != null) {
                Instantiate(blood, hit.point, Quaternion.identity);
            }

            if (audioSource != null) {
                audioSource.PlayOneShot(auchSound);
            }
        }
    }
}