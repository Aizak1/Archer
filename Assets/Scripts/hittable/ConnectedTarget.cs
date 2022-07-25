using arrow;
using UnityEngine;

namespace hittable {
    public class ConnectedTarget : Target {
        [SerializeField] private ConnectedTarget[] targets;
        [SerializeField] private float timeBetweenHits;

        private float hitTime;

        private void Start() {
            hitTime = float.MinValue;
        }

        public override void ProcessHit(Arrow arrow, RaycastHit hit) {
            hitTime = Time.time;
            int targetsLength = targets.Length;
            for (var i = 0; i < targetsLength; i++)
            {
                var target = targets[i];
                float deltaTime = hitTime - target.hitTime;

                if (deltaTime > timeBetweenHits)
                {
                    return;
                }
            }

            foreach (var target in targets) {
                target.ProcessHitBase(arrow,hit);
            }

            ProcessHitBase(arrow,hit);
        }

        private void ProcessHitBase(Arrow arrow, RaycastHit hit)
        {
            base.ProcessHit(arrow, hit);
        }
    }
}