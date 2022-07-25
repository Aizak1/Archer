using portal;
using UnityEngine;

namespace arrow.behaviours
{
    public class PortalArrowBehavior: MonoBehaviour, IAdditionalArrowBehavior
    {
        public void Fly()
        {
            
        }

        public void HitProcessing(RaycastHit hit)
        {
           PortalController.Instance.CreatePortal(hit);
           Destroy(gameObject);
        }

        public void Release(Vector3 velocity)
        {
        
        }

        public bool IsDisabled { get; set; }
        public BehaviorType BehaviorType { get; set; } = BehaviorType.Portal;
    }
}