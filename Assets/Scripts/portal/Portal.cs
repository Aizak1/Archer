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

        private const string OPEN_PORTAL_TRIGGER = "Open";
        private const string CLOSE_PORTAL_TRIGGER = "Close";

        private const float CLOSE_ANIMATION_TIME = 0.5f;
        private const int LAST_MATRIX_COLUMN = 3;
        public const float PORTAL_SPAWN_OFFSET = 8.7f;

        private const string UNTAGGED = "Untagged";
        public const string BLUE_PORTAL_TAG = "Blue Portal";
        public const string ORANGE_PORTAL_TAG = "Orange Portal";

        public const string SLICE_DIRECTION = "_V3SliceLocalDirection";
        public const string SLICE_STAGE = "_SliceStage";

        private List<PortalTraveller> trackedTravellers;
        private PortalCameraRenderer cameraRenderer;

        private void Awake() {
            trackedTravellers = new List<PortalTraveller>();
            cameraRenderer = FindObjectOfType<PortalCameraRenderer>();
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

        private void CheckSecondPortal() {
            string tagToFindObject;

            if (isOrange) {
                tagToFindObject = BLUE_PORTAL_TAG;
            } else {
                tagToFindObject = ORANGE_PORTAL_TAG;
            }

            var portalObject = GameObject.FindGameObjectWithTag(tagToFindObject);
            if (portalObject == null) {
                return;
            }
            var portal = portalObject.GetComponent<Portal>();

            if (portal == null) {
                return;
            }

            ConnectPortals(portal);
            linkedPortal.ConnectPortals(this);
        }

        private void ConnectPortals(Portal portal) {
            linkedPortal = portal;
            isReady = true;
        }

        public void Open() {
            GetComponent<Animator>().SetTrigger(OPEN_PORTAL_TRIGGER);
            CheckSecondPortal();
            if(cameraRenderer == null) {
                Debug.LogError("Add Portal camera renderer");
                return;
            }
            cameraRenderer.portals.Add(this);

            audioSource.PlayOneShot(portalSound);
        }

        public void Close() {
            GetComponent<Collider>().enabled = false;
            tag = UNTAGGED;
            GetComponent<Animator>().SetTrigger(CLOSE_PORTAL_TRIGGER);
            Destroy(transform.parent.gameObject, CLOSE_ANIMATION_TIME);

            if (cameraRenderer == null) {
                Debug.LogError("Add Portal camera renderer");
                return;
            }
            cameraRenderer.portals.Remove(this);
        }
    }
}
