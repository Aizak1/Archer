using System;
using System.Collections.Generic;
using System.Linq;
using arrow.behaviours;
using hittable;
using portal;
using bow;
using UnityEngine;
using UnityEngine.Events;

namespace arrow {
    
    public class Arrow : MonoBehaviour {
        
        [SerializeField] private float _speed;
        
        [SerializeField] protected new Rigidbody _rigidbody;
        [SerializeField] private new Collider _collider;
        [SerializeField] protected TrailRenderer _trailRenderer;
        [SerializeField] private Transform tip;

        [HideInInspector] public bool isTeleporting;

        private Vector3 _lastTipPosition;
        private const float TIP_POS_ACCURACY = 1;
        private const string RAYCAST_LAYER = "Default";
        private int _mask;

        private List<IAdditionalArrowBehavior> _additionalArrowBehaviors;

        public UnityAction<RaycastHit> OnHit;
        public UnityAction OnRelease;

        public float Speed => _speed;
        public TrailRenderer TrailRenderer => _trailRenderer;
        public Rigidbody Rigidbody => _rigidbody;
        public List<IAdditionalArrowBehavior> AdditionalArrowBehaviors => _additionalArrowBehaviors;

        private void Awake()
        {
            _lastTipPosition = tip.transform.position;
            _trailRenderer.enabled = false;
            _mask = LayerMask.GetMask(RAYCAST_LAYER);
            
            _additionalArrowBehaviors = GetComponents<IAdditionalArrowBehavior>().ToList();
        }

        private void Start()
        {
            for (int i = 0; i < _additionalArrowBehaviors.Count; i++)
            {
                if (_additionalArrowBehaviors[i].IsDisabled)
                {
                    _additionalArrowBehaviors.RemoveAt(i);
                    i--;
                }
            }
        }

        private void Update()
        {
            if (IsHitSomething(out RaycastHit hit))
            {
                HitProcessing(hit);
            }
            else
            {
                Fly();
            }
        }

        private void Fly()
        { 
            transform.rotation = Quaternion.LookRotation(_rigidbody.velocity, transform.up);
            _lastTipPosition = transform.position;

            foreach (var item in _additionalArrowBehaviors)
            {
              item.Fly();   
            }
        }

        private bool IsHitSomething(out RaycastHit raycastHit)
        {
            raycastHit = new RaycastHit();
            var tipDistance = (_lastTipPosition - tip.position).magnitude;   
            if (tipDistance < TIP_POS_ACCURACY &&
                Physics.Linecast(_lastTipPosition, tip.position, out RaycastHit hit, _mask))
            {
                raycastHit = hit;
                return true;
            }
           
            return false;
        }

        private  void HitProcessing(RaycastHit hit)
        {
            Destroy(_rigidbody);
            enabled = false;

            _trailRenderer.enabled = false;
            _collider.enabled = false;

            var hittable = hit.collider.GetComponent<Hittable>();
            if (hittable)
            {
                hittable.ProcessHit(this);
            }
            OnHit?.Invoke(hit);
            foreach (var item in _additionalArrowBehaviors)
            {
                item.HitProcessing(hit);   
            }
        }

        public  void Release(Vector3 velocity)
        {
            _trailRenderer.enabled = true;
            _rigidbody.useGravity = true;
            _rigidbody.isKinematic = false;
            _rigidbody.velocity = velocity;
            
            foreach (var item in _additionalArrowBehaviors)
            {
                item.Release(velocity);   
            }
        }
    }
}