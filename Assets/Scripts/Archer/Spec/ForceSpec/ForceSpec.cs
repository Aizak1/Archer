using System;
using UnityEngine;

namespace Archer.Specs.Force {
    [Serializable]
    public struct ForceSpec {
        public ForceSpec(float force,
            Vector3 forceDirection, ForceMode forceMode, bool onStart, bool onUpdate) {
            Force = force;
            ForceDirection = forceDirection;
            ForceMode = forceMode;
            OnStart = onStart;
            OnUpdate = onUpdate;
        }

        public bool OnStart;
        public bool OnUpdate;
        public float Force;
        public Vector3 ForceDirection;
        public ForceMode ForceMode;
    }

    [Serializable]
    public struct VelocitySpec {
        public VelocitySpec(
            Vector3 velocity,
            float velocityMultiplyer,
            bool isConstVelocity,
            bool isStartVelocity,
            bool isVelocityAcceleration) {
            this.velocity = velocity;
            this.velocityMultiplyer = velocityMultiplyer;
            this.isConstVelocity = isConstVelocity;
            this.isStartVelocity = isStartVelocity;
            this.isVelocityAcceleration = isVelocityAcceleration;
        }

        [SerializeField] private Vector3 velocity;
        [SerializeField] private float velocityMultiplyer;
        [SerializeField] private bool isConstVelocity;
        [SerializeField] private bool isStartVelocity;
        [SerializeField] private bool isVelocityAcceleration;

        public Vector3 Velocity => velocity;
        public float Velocitymultiplyer => velocityMultiplyer;
        public bool IsConstVelocity => isConstVelocity;
        public bool IsStartVelocity => isStartVelocity;
        public bool IsVelocityAcceleration => isVelocityAcceleration;


    }
}
