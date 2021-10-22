using System;
using Archer.Types.ArrowTypes;
using UnityEngine;

namespace Archer.Specs.Arrow {
    [Serializable]
    public struct ArrowSpec {
        public ArrowSpec(
            ArrowTypes arrowType,
            float penetrationFactor,
            bool isSplitable,
            float splitAngle,
            int splitCount) {
            this.arrowType = arrowType;
            this.penetrationFactor = penetrationFactor;
            this.isSplitable = isSplitable;
            this.splitAngle = splitAngle;
            this.splitCount = splitCount;
        }

        [SerializeField] private ArrowTypes arrowType;
        public ArrowTypes ArrowType => arrowType;

        [SerializeField] private float penetrationFactor;
        public float PenetrationFactor => penetrationFactor;

        [SerializeField] private bool isSplitable;
        public bool IsSplitable => isSplitable;

        [SerializeField] private float splitAngle;
        public float SplitAngle => splitAngle;

        [SerializeField] private int splitCount;
        public int SplitCount => splitCount;
    }
}
