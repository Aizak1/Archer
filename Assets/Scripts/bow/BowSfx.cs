using System;
using UnityEngine;

namespace bow
{
    [RequireComponent(typeof(Bow),typeof(AudioSource))]
    public class BowSfx: MonoBehaviour
    {
        [SerializeField] private AudioClip _shootSound;
        [SerializeField] private AudioClip _pullingSound;
        
        private AudioSource _audioSource;
        private Bow _bow;

        private void Awake()
        {
            _bow = GetComponent<Bow>();
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            _bow.OnStartPull += PlayPullingSound;
            _bow.OnEndPull += PlayReleaseSound;
        }

        private void OnDisable()
        {
            _bow.OnStartPull -= PlayPullingSound;
            _bow.OnEndPull -= PlayReleaseSound;
        }

        private void PlayReleaseSound()
        {
            if (_audioSource.isPlaying) {
                _audioSource.Stop();
            }
            _audioSource.PlayOneShot(_shootSound);
        }

        private void PlayPullingSound()
        {
            if (_audioSource.isPlaying) {
                _audioSource.Stop();
            }
            _audioSource.PlayOneShot(_pullingSound);
        }
    }
}