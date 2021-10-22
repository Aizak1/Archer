using System;

namespace Archer.Specs.Hitable {
    [Serializable]
    public struct HitableSpec {
        //TODO remake
        public HitableSpec(
            float hitAbsorbation,
            float absorbationPerUnit,
            float penetrationThreshold, 
            float ricochetTreshold) {
            HitAbsorbation = hitAbsorbation;
            AbsorbationPerUnit = absorbationPerUnit;
            PenetrationThreshold = penetrationThreshold;
            RicochetTreshold = ricochetTreshold;
        }

        public float HitAbsorbation;
        public float AbsorbationPerUnit;
        public float PenetrationThreshold;
        public float RicochetTreshold;
    }
}
