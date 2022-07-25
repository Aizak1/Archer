using arrow;
using UnityEngine;

namespace hittable
{
    public interface IHittable
    {
        public void ProcessHit(Arrow arrow, RaycastHit hit);
    }
}