using System;
using UnityEngine;

namespace Archer.Specs.Hitable {
    [Serializable]
    public struct HitableSpec {
        public HitableSpec(
            float hitAbsorbation,
            float absorbationPerUnit,
            float penetrationThreshold, 
            float ricochetTreshold,
            bool isEndless) {
            HitAbsorbation = hitAbsorbation;
            AbsorbationPerUnit = absorbationPerUnit;
            PenetrationThreshold = penetrationThreshold;
            RicochetTreshold = ricochetTreshold;
            IsEndless = isEndless;
        }

        public float HitAbsorbation;
        public float AbsorbationPerUnit;
        public float PenetrationThreshold;
        public float RicochetTreshold;
        public bool IsEndless;
    }
}
