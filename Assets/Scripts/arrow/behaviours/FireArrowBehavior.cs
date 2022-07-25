using UnityEngine;

namespace arrow.behaviours
{
    public class FireArrowBehavior : MonoBehaviour, IAdditionalArrowBehavior
    {
        public void Fly()
        {
            
        }

        public void HitProcessing(RaycastHit hit)
        {
            
        }

        public void Release(Vector3 velocity)
        {
           
        }

        public bool IsDisabled { get; set; }
        public BehaviorType BehaviorType { get; set; } = BehaviorType.Fire;
    }
}