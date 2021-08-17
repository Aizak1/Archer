using System.Collections.Generic;
using UnityEngine;

namespace portal {
    public class PortalTraveller : MonoBehaviour {
        public GameObject graphicsObject;

        [HideInInspector]
        public GameObject graphicsClone;

        [HideInInspector]
        public Vector3 previousOffsetFromPortal;

        [HideInInspector]
        public Material[] originalMaterials;

        [HideInInspector]
        public Material[] cloneMaterials;

        [HideInInspector]
        public new Rigidbody rigidbody;

        [HideInInspector]
        public int sign;

        private void Start() {
            rigidbody = GetComponent<Rigidbody>();
        }

        public void Teleport(Transform portal, Transform toPortal, Vector3 pos, Quaternion rot) {
            transform.position = pos;
            transform.rotation = rot;

            if (rigidbody == null) {
                return;
            }
            var inverseVel = portal.InverseTransformVector(rigidbody.velocity);
            var vel = toPortal.transform.TransformVector(inverseVel);
            rigidbody.velocity = vel;

            var inverseAngularVel = portal.InverseTransformVector(rigidbody.angularVelocity);
            var angVel = toPortal.transform.TransformVector(inverseAngularVel);
            rigidbody.angularVelocity = angVel;
        }

        public void EnterPortalThreshold() {
            if (graphicsClone == null) {
                graphicsClone = Instantiate(graphicsObject);
                graphicsClone.transform.parent = graphicsObject.transform.parent;
                graphicsClone.transform.localScale = graphicsObject.transform.localScale;
                originalMaterials = GetMaterials(graphicsObject);
                cloneMaterials = GetMaterials(graphicsClone);
            } else {
                graphicsClone.SetActive(true);
            }
        }

        public virtual void ExitPortalThreshold() {
            graphicsClone.SetActive(false);
            for (int i = 0; i < originalMaterials.Length; i++) {
                originalMaterials[i].SetVector(Portal.SLICE_NORMAL, Vector3.zero);
            }
        }

        private Material[] GetMaterials(GameObject gameObject) {
            var renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
            var matList = new List<Material>();
            foreach (var renderer in renderers) {
                matList.AddRange(renderer.materials);
            }
            var matArray = new Material[matList.Count];
            for (int i = 0; i < matList.Count; i++) {
                matArray[i] = matList[i];
            }
            return matArray;
        }
    }
}