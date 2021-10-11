using System;
using UnityEngine;

namespace Archer.Specs.Rigid {
    [Serializable]
    public struct RigidSpec {
        public RigidSpec(
            bool isChaneParams,
            float mass, float drag, float angularDrag,
            bool isChangeFlags,
            bool isKinematic, bool useGravity,
            bool isChangeConstraints,
            bool freezePosX, bool freezePosY, bool freezePosZ,
            bool freezeRotX, bool freezeRotY, bool freezeRotZ) {
            IsChaneParams = isChaneParams;
            Mass = mass;
            Drag = drag;
            AngularDrag = angularDrag;
            Drag = drag;
            IsChangeFlags = isChangeFlags;
            IsKinematic = isKinematic;
            UseGravity = useGravity;
            IsChangeConstraints = isChangeConstraints;
            FreezePosX = freezePosX;
            FreezePosY = freezePosY;
            FreezePosZ = freezePosZ;
            FreezeRotX = freezeRotX;
            FreezeRotY = freezeRotY;
            FreezeRotZ = freezeRotZ;
        }

        [Header("Params")]
        public bool IsChaneParams;
        public float Mass;
        public float Drag;
        public float AngularDrag;
        [Header("ChangeFlags")]
        public bool IsChangeFlags;
        public bool IsKinematic;
        public bool UseGravity;
        [Header("ChangeConstraints")]
        public bool IsChangeConstraints;
        public bool FreezePosX;
        public bool FreezePosY;
        public bool FreezePosZ;
        public bool FreezeRotX;
        public bool FreezeRotY;
        public bool FreezeRotZ;
    }
}
