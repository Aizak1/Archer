using System;
using arrow.sfx;
using UnityEngine;
using UnityEngine.Events;

namespace arrow.behaviours
{
    public class SplitArrowBehavior: MonoBehaviour, IAdditionalArrowBehavior
    {
        [SerializeField] private int _splitArrowsAmount;
        [SerializeField]private float _timeBeforeSplit;
        [SerializeField] private float _angleBetweenSplitArrows;
        private float _splitTime;

        [SerializeField] private ParticleSystem _splitEffect;
        [SerializeField] private AudioClip _splitSound;
        
        private Arrow _arrow;
        
        
        private void Awake()
        {
            _arrow = GetComponent<Arrow>();
        }
        
        public void Fly()
        {
            if (Time.time >= _splitTime && !_arrow.isTeleporting)
            {
                Instantiate(_splitEffect, transform.position, Quaternion.identity);
                Split(_angleBetweenSplitArrows, _splitArrowsAmount);
            }
        }

        public void HitProcessing(RaycastHit hit)
        {
            enabled = false;
        }

        public void Release(Vector3 velocity)
        {
            _splitTime = Time.time + _timeBeforeSplit;
        }

        public bool IsDisabled { get; set; }
        
        public BehaviorType BehaviorType { get; set; } = BehaviorType.Split;


        private void Split(float angleBetweenSplitArrows, int splitArrowsAmount) 
        {
            float angle = -angleBetweenSplitArrows * (splitArrowsAmount - 1) / 2;
            Arrow instantiatedArrow = null;
            for (int i = 0; i < splitArrowsAmount; i++) {
                instantiatedArrow = Instantiate(_arrow, transform.position, transform.rotation);
                Vector3 velocity = _arrow.Rigidbody.velocity;

                float radAngle = angle * Mathf.Deg2Rad;

                float newY = Mathf.Sin(radAngle) * velocity.z + Mathf.Cos(radAngle) * velocity.y;
                float newZ = Mathf.Cos(radAngle) * velocity.z - Mathf.Sin(radAngle) * velocity.y;

                Vector3 newVelocity = new Vector3(velocity.x, newY, newZ);

                foreach (var item in instantiatedArrow.AdditionalArrowBehaviors)
                {
                    if (item.BehaviorType == BehaviorType.Split)
                    {
                        item.IsDisabled = true;
                    }
                }
                instantiatedArrow.Release(newVelocity);

                angle += angleBetweenSplitArrows;
            }
            instantiatedArrow.GetComponent<ArrowSfx>()?.PlaySound(_splitSound);
            Destroy(gameObject);
        }
    }
}