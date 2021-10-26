using System.Collections.Generic;
using Archer.Specs.Force;
using UnityEngine;

namespace Archer.Controlls.Other {
    [RequireComponent(typeof(Rigidbody))]
    public class ForceControler : MonoBehaviour {
        [SerializeField] private List<ForceSpec> forceSpecList;
        [SerializeField] private List<VelocitySpec> velocitySpecList;

        private Rigidbody rigid;

        private void Start() {
            rigid = GetComponent<Rigidbody>();
            foreach (var forceDesc in forceSpecList) {
                if (forceDesc.OnStart) {
                    rigid.AddForce(
                        forceDesc.ForceDirection * forceDesc.Force, forceDesc.ForceMode);
                }
            }
            foreach (var velocitySpec in velocitySpecList) {
                if (velocitySpec.IsStartVelocity) {
                    rigid.velocity = velocitySpec.Velocity * velocitySpec.Velocitymultiplyer;
                }
            }
        }

        private void FixedUpdate()
        {
            foreach (var forceDesc in forceSpecList) {
                if (forceDesc.OnUpdate) {
                    rigid.AddForce(
                        forceDesc.ForceDirection * forceDesc.Force, forceDesc.ForceMode);
                }
            }

            var constantVelocitySumm = Vector3.zero;
            foreach (var velocitySpec in velocitySpecList) {
                if (velocitySpec.IsVelocityAcceleration) {
                    rigid.velocity += velocitySpec.Velocity * velocitySpec.Velocitymultiplyer;
                }
                if (velocitySpec.IsConstVelocity) {
                    constantVelocitySumm += velocitySpec.Velocity * velocitySpec.Velocitymultiplyer;
                }
            }
            if (constantVelocitySumm != Vector3.zero) {
                rigid.velocity = constantVelocitySumm;
            }
        }
    }
}
