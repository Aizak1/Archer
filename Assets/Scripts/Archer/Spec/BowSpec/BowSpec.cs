using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archer.Specs.Bow {
    [Serializable]
    public struct BowSpec {
        public float MaxForce;
        public float ForceIncrement;
        public float ForveDecrement;
        public float AngleChange;

        public BowSpec(
            float maxForce,
            float forceIncrement,
            float forveDecrement,
            float angleChange) {
            MaxForce = maxForce;
            ForceIncrement = forceIncrement;
            ForveDecrement = forveDecrement;
            AngleChange = angleChange;
        }
    }
}

