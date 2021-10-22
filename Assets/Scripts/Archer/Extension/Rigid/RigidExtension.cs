using System.Collections;
using System.Collections.Generic;
using Archer.Specs.Rigid;
using UnityEngine;

namespace Archer.Extension.Rigid {
    public static class RigidExtension {
        public static void ApplyToRigid(this RigidSpec rigidSpec, Rigidbody rigid) {
            if (rigid == null)
                return;

            if (rigidSpec.IsChaneParams) {
                rigid.mass = rigidSpec.Mass;
                rigid.drag = rigidSpec.Drag;
                rigid.angularDrag = rigidSpec.AngularDrag;
            }

            if (rigidSpec.IsChangeFlags) {
                rigid.isKinematic = rigidSpec.IsKinematic;
                rigid.useGravity = rigidSpec.UseGravity;
            }

            rigid.constraints = RigidbodyConstraints.None;
            if (rigidSpec.IsChangeConstraints) {
                var constrain = RigidbodyConstraints.None;
                if (rigidSpec.FreezePosX)
                    constrain |= RigidbodyConstraints.FreezePositionX;
                if (rigidSpec.FreezePosY)
                    constrain |= RigidbodyConstraints.FreezePositionY;
                if (rigidSpec.FreezePosZ)
                    constrain |= RigidbodyConstraints.FreezePositionZ;
                if (rigidSpec.FreezeRotX)
                    constrain |= RigidbodyConstraints.FreezeRotationX;
                if (rigidSpec.FreezeRotY)
                    constrain |= RigidbodyConstraints.FreezeRotationY;
                if (rigidSpec.FreezeRotZ)
                    constrain |= RigidbodyConstraints.FreezeRotationZ;
                rigid.constraints = constrain;
            }

            return;
        }
    }
}

