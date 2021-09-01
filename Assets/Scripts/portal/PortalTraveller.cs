using System.Collections.Generic;
using UnityEngine;

namespace portal {
    public class PortalTraveller : MonoBehaviour {
        public GameObject graphicsObject;

        public Transform tipTransform;
        public Transform tailTransform;

        [HideInInspector]
        public float length;

        [HideInInspector]
        public GameObject graphicsClone;

        [HideInInspector]
        public Material[] originalMaterials;

        [HideInInspector]
        public Material[] cloneMaterials;

        [SerializeField]
        public new Rigidbody rigidbody;

        private Vector3 originalSliceDirection = new Vector3(0, -1, 0);
        private Vector3 cloneSliceDirection = new Vector3(0, 1, 0);

        private void Start() {
            length = (tipTransform.position - tailTransform.position).magnitude;
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

                for (int i = 0; i < originalMaterials.Length; i++) {
                    originalMaterials[i].SetVector(Portal.SLICE_DIRECTION, originalSliceDirection);
                    cloneMaterials[i].SetVector(Portal.SLICE_DIRECTION, cloneSliceDirection);
                }
            } else {
                graphicsClone.SetActive(true);
            }
        }

        public virtual void ExitPortalThreshold() {
            graphicsClone.SetActive(false);
            for (int i = 0; i < originalMaterials.Length; i++) {
                originalMaterials[i].SetVector(Portal.SLICE_DIRECTION, Vector3.zero);
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