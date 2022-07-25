using arrow;
using UnityEngine;
using UnityEngine.Events;

namespace hittable
{
    public class Target: MonoBehaviour, IHittable
    {
        public UnityEvent OnTargetHitted; 
        public virtual void ProcessHit(Arrow arrow, RaycastHit hit)
        {
            OnTargetHitted?.Invoke();
            Destroy(this);
        }
    }
}