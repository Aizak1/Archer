using UnityEngine;

namespace arrow.behaviours
{
    public interface IAdditionalArrowBehavior
    {
        void Fly();

        void HitProcessing(RaycastHit hit);

        void Release(Vector3 velocity);

        public bool IsDisabled { get; set; }
        
        public BehaviorType BehaviorType { get; set; }
    }
}