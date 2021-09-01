using System.Collections.Generic;
using UnityEngine;
using arrow;

namespace portal {

    public class Portal : MonoBehaviour {
        public bool isOrange;

        public Portal linkedPortal;

        public bool isReady = false;

        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip portalSound;

        [SerializeField]
        private new Collider collider;
        [SerializeField]
        private Animator animator;

        private const string OPEN_PORTAL_TRIGGER = "Open";
        private const string CLOSE_PORTAL_TRIGGER = "Close";

        private const float CLOSE_ANIMATION_TIME = 0.5f;
        private const int LAST_MATRIX_COLUMN = 3;
        public const float PORTAL_SPAWN_OFFSET = 8.7f;

        public const string SLICE_DIRECTION = "_V3SliceLocalDirection";
        public const string SLICE_STAGE = "_SliceStage";

        private List<PortalTraveller> trackedTravellers;
        private PortalCameraRenderer cameraRenderer;

        private void Awake() {
            trackedTravellers = new List<PortalTraveller>();
        }

        private void LateUpdate() {
            for (int i = 0; i < trackedTravellers.Count; i++) {
                PortalTraveller traveller = trackedTravellers[i];
                if (traveller == null) {
                    trackedTravellers.Remove(traveller);
                    i--;
                    continue;
                }

                if (traveller.rigidbody.velocity == Vector3.zero) {
                    ExitTeleport(traveller);
                    return;
                }

                Transform travellerTransform = traveller.transform;
                var linkWorldMatrix = linkedPortal.transform.localToWorldMatrix;
                var localMatrix = transform.worldToLocalMatrix;
                var travellerWorldMatrix = travellerTransform.localToWorldMatrix;

                var matrix = linkWorldMatrix * localMatrix * travellerWorldMatrix;

                var pos = matrix.GetColumn(LAST_MATRIX_COLUMN);
                var rot = matrix.rotation;
                traveller.graphicsClone.transform.SetPositionAndRotation(pos, rot);
            }
        }

        public void PortalRenderer() {
            for (int i = 0; i < trackedTravellers.Count; i++) {
                if (trackedTravellers[i]) {
                    UpdateSliceParams(trackedTravellers[i]);
                }
            }
        }

        private void UpdateSliceParams(PortalTraveller traveller) {

            float dst = (transform.position - traveller.tailTransform.position).magnitude;
            float stage = dst / traveller.length;

            if (stage <= 0.8f) {
                ExitTeleport(traveller);
                return;
            }

            for (int i = 0; i < traveller.originalMaterials.Length; i++) {
                traveller.originalMaterials[i].SetFloat(SLICE_STAGE, stage);

                traveller.cloneMaterials[i].SetFloat(SLICE_STAGE, 1 - stage);
            }
        }

        private void ProcessTravellerEnterPortal(PortalTraveller traveller) {
            if (!trackedTravellers.Contains(traveller)) {
                traveller.EnterPortalThreshold();
                trackedTravellers.Add(traveller);
                PortalRenderer();
            }
        }

        public void StartPortalTravelling(Collider other) {
            if (!isReady) {
                return;
            }

            var traveller = other.GetComponent<PortalTraveller>();
            if (traveller == null) {
                return;
            }

            Transform travellerTransform = traveller.transform;
            var linkWorldMatrix = linkedPortal.transform.localToWorldMatrix;
            var localMatrix = transform.worldToLocalMatrix;
            var travellerWorldMatrix = travellerTransform.localToWorldMatrix;

            var matrix = linkWorldMatrix * localMatrix * travellerWorldMatrix;

            var positionOld = travellerTransform.position;
            var rotOld = travellerTransform.rotation;

            var pos = matrix.GetColumn(LAST_MATRIX_COLUMN);
            var rot = matrix.rotation;

            traveller.Teleport(transform, linkedPortal.transform, pos, rot);
            traveller.EnterPortalThreshold();
            traveller.graphicsClone.transform.SetPositionAndRotation(positionOld, rotOld);

            linkedPortal.ProcessTravellerEnterPortal(traveller);
        }

        private void OnTriggerExit(Collider other) {
            if (!isReady) {
                return;
            }
            var traveller = other.GetComponent<PortalTraveller>();
            if (traveller) {
                ExitTeleport(traveller);
            }
        }

        private void ExitTeleport(PortalTraveller traveller) {
            if (traveller && trackedTravellers.Contains(traveller)) {

                for (int i = 0; i < traveller.originalMaterials.Length; i++) {
                    traveller.originalMaterials[i].SetFloat(SLICE_STAGE, 0);

                    traveller.cloneMaterials[i].SetFloat(SLICE_STAGE, 1);
                }

                traveller.ExitPortalThreshold();
                trackedTravellers.Remove(traveller);

                var arrow = traveller.GetComponent<Arrow>();

                if (arrow != null) {
                    arrow.trailRenderer.enabled = true;
                    arrow.isTeleporting = false;
                }
            }
        }

        private void CheckSecondPortal(Portal portal) {
            ConnectPortals(portal);
            linkedPortal.ConnectPortals(this);
        }

        private void ConnectPortals(Portal portal) {
            linkedPortal = portal;
            isReady = true;
        }

        public void Open(PortalCameraRenderer cameraRenderer, Portal portalToLink = null) {
            animator.SetTrigger(OPEN_PORTAL_TRIGGER);
            if (portalToLink) {
                CheckSecondPortal(portalToLink);
            }
            this.cameraRenderer = cameraRenderer;
            this.cameraRenderer.portals.Add(this);

            audioSource.PlayOneShot(portalSound);
        }

        public void Close() {
            collider.enabled = false;
            animator.SetTrigger(CLOSE_PORTAL_TRIGGER);
            Destroy(transform.parent.gameObject, CLOSE_ANIMATION_TIME);

            if (cameraRenderer == null) {
                Debug.LogError("Add Portal camera renderer");
                return;
            }
            cameraRenderer.portals.Remove(this);
        }
    }
}