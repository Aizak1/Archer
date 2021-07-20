using arrow;
using UnityEngine;

namespace hittable {
    public class Target : MonoBehaviour, IHittable {
        [SerializeField]
        private Transform center;
        [SerializeField]
        private Transform edge;

        [SerializeField]
        private int[] points;

        private float[] sectors;

        private void Start() {
            float sectorWidth = (center.position - edge.position).magnitude / points.Length;

            sectors = new float[points.Length];

            float sectorStart = 0;

            for (int i = 0; i < sectors.Length; i++) {
                sectorStart += sectorWidth;
                sectors[i] = sectorStart;
            }
        }

        public int CalculatePoints(Vector3 hitPoint) {
            float distanceFormCenter = (center.position - hitPoint).magnitude;

            for (int i = 0; i < sectors.Length; i++) {
                if (sectors[i] < distanceFormCenter) {
                    continue;
                }
                return points[i];
            }

            return default;
        }

        public void ProcessHit(Arrow arrow, RaycastHit hit) {
            var points = CalculatePoints(hit.point);
            Debug.Log(points);
        }
    }
}

