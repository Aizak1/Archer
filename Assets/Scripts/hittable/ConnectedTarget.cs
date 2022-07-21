using UnityEngine;

namespace hittable {
    [RequireComponent(typeof(Hittable))]
    public class ConnectedTarget : MonoBehaviour {
        [SerializeField]
        private Hittable hittable;

        [SerializeField]
        private ConnectedTarget[] targets;

        [SerializeField]
        private float timeBetweenHits;

        private float hitTime;

        private void Start() {
            hitTime = float.MinValue;
        }

        public void ProcessHit() {
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
                Destroy(target);
                hittable.levelController.DecreaseEnemyCount();
            }

            hittable.levelController.DecreaseEnemyCount();
            Destroy(this);
        }
    }
}