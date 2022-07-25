using System;
using arrow;
using UnityEngine;

namespace hittable {
    public class MovingTarget : Target
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public override void ProcessHit(Arrow arrow, RaycastHit hit)
        {
            _animator.speed = 0;
            base.ProcessHit(arrow, hit);
        }
        
    }
}
