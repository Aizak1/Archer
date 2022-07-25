using arrow;
using hittable;
using UnityEngine;
using level;

namespace player {
    public class Player : MonoBehaviour, IHittable {
        [SerializeField]
        private LevelController levelController;
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip auchSound;
        [SerializeField]
        private ParticleSystem blood;

        public void ProcessHit(Arrow arrow, RaycastHit hit)
        {
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