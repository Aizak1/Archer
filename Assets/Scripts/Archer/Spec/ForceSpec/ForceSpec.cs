using System;
using UnityEngine;

namespace Archer.Specs.Force {
    [Serializable]
    public struct ForceSpec {
        public ForceSpec(float force, Vector3 forceDirection, ForceMode forceMode) {
            Force = force;
            ForceDirection = forceDirection;
            ForceMode = forceMode;
        }

        public float Force;
        public Vector3 ForceDirection;
        public ForceMode ForceMode;
    }
}
