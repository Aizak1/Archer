using System.Collections.Generic;
using Archer.Specs.Force;
using UnityEngine;

namespace Archer.Controlls.Other {
    [RequireComponent(typeof(Rigidbody))]
    public class ForceControler : MonoBehaviour {
        [SerializeField] private List<ForceSpec> forceSpecList;

        private Rigidbody rigid;

        private void Start() {
            rigid = GetComponent<Rigidbody>();
            foreach (var forceDesc in forceSpecList) {
                if (forceDesc.ForceMode == ForceMode.Acceleration ||
                    forceDesc.ForceMode == ForceMode.Force) {
                    rigid.AddForce(
                        forceDesc.ForceDirection * forceDesc.Force, forceDesc.ForceMode);
                }
            }
        }

        private void FixedUpdate()
        {
            foreach (var forceDesc in forceSpecList) {
                if (forceDesc.ForceMode == ForceMode.Impulse ||
                    forceDesc.ForceMode == ForceMode.VelocityChange) {
                    rigid.AddForce(
                        forceDesc.ForceDirection * forceDesc.Force, forceDesc.ForceMode);
                }
            }
        }
    }
}
