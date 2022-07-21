using System;
using UnityEngine;

namespace arrow.sfx
{
    [RequireComponent(typeof(AudioSource),typeof(Arrow))]
    public class ArrowSfx : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _impactSound;

        private Arrow _arrow;
        private AudioSource _audioSource;

        private void Awake()
        {
            _arrow = GetComponent<Arrow>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            _arrow.OnHit += PlayImpactSound;
        }

        private void OnDisable()
        {
            _arrow.OnHit -= PlayImpactSound;
        }

        private void PlayImpactSound(RaycastHit hit)
        {
            _audioSource.PlayOneShot(_impactSound);
        }

        public void PlaySound(AudioClip audioClip)
        {
            _audioSource.PlayOneShot(audioClip);
        }
    }
}