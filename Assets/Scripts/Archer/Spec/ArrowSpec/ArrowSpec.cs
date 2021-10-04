using Archer.Types.ArrowTypes;
using System;

namespace Archer.Specs.Arrow {
    [Serializable]
    public struct ArrowSpec {
        public ArrowTypes ArrowType;
        public float ArrowMass;
        public float PenetrationFactor;
        public bool IsSplitable;
        public float SplitAngle;
        public int SplitCount;

        public ArrowSpec(
            ArrowTypes arrowType,
            float arrowMass,
            float penetrationFactor,
            bool isSplitable,
            float splitAngle,
            int splitCount) {
            ArrowType = arrowType;
            ArrowMass = arrowMass;
            PenetrationFactor = penetrationFactor;
            IsSplitable = isSplitable;
            SplitAngle = splitAngle;
            SplitCount = splitCount;
        }
    }
}
