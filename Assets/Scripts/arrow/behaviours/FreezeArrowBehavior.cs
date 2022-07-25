using UnityEngine;

namespace arrow.behaviours
{
    public class FreezeArrowBehavior : MonoBehaviour, IAdditionalArrowBehavior
    {
        public void Fly()
        {
            throw new System.NotImplementedException();
        }

        public void HitProcessing(RaycastHit hit)
        {
            throw new System.NotImplementedException();
        }

        public void Release(Vector3 velocity)
        {
            throw new System.NotImplementedException();
        }

        public bool IsDisabled { get; set; }
        public BehaviorType BehaviorType { get; set; } = BehaviorType.Freeze;
    }
}