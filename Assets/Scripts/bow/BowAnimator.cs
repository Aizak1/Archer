using System;
using UnityEngine;

namespace bow
{
    public class BowAnimator : MonoBehaviour
    {
        [SerializeField] private float _maxPull = 0.93f;
        private readonly int _blendID = Animator.StringToHash("Blend");
        private Bow _bow;
        private Animator _bowAnimator;

        private void Awake()
        {
            _bow = GetComponent<Bow>();
            _bowAnimator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _bow.OnPull += OnPull;
            _bow.OnEndPull += OnEndPull;
        }

        private void OnDisable()
        {
            _bow.OnPull -= OnPull;
            _bow.OnEndPull -= OnEndPull;
        }
        

        private void OnPull()
        {
            SetPullToTheAnimator();
        }

        private void OnEndPull()
        {
            SetPullToTheAnimator();
        }

        private void SetPullToTheAnimator()
        {
            _bowAnimator.SetFloat(_blendID, _bow.pullAmount * _maxPull);
        }
    }
}