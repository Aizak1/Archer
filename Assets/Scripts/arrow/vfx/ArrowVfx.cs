using System;
using UnityEngine;

namespace arrow.vfx
{
    [RequireComponent(typeof(Arrow))]
    public class ArrowVfx : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem _tailVfx;
        [SerializeField]
        private ParticleSystem _hitVfx;

        private const float VFX_LIFE_AFTER_HIT = 0.3f;
        
        private Arrow _arrow;

        private void Awake()
        {
            _arrow = GetComponent<Arrow>();
        }

        private void OnEnable()
        {
            _arrow.OnHit += CreateParticlesOnHit;
        }

        private void OnDisable()
        {
            _arrow.OnHit -= CreateParticlesOnHit;
        }

        private void CreateParticlesOnHit(RaycastHit hit)
        {
            Instantiate(_hitVfx, hit.point, Quaternion.LookRotation(hit.normal));
            if (_tailVfx != null) {
                Destroy(_tailVfx, VFX_LIFE_AFTER_HIT);
            }
        }
    }
}