﻿using System;
using System.Collections.Generic;
using UnityEngine;
using arrow;

namespace portal {

    public class Portal : MonoBehaviour {
        public bool isOrange;

        public Portal linkedPortal;

        public bool isReady = false;

        private const string OPEN_PORTAL_TRIGGER = "Open";
        private const string CLOSE_PORTAL_TRIGGER = "Close";

        private const float CLOSE_ANIMATION_TIME = 0.5f;
        private const int LAST_MATRIX_COLUMN = 3;
        public const float PORTAL_SPAWN_OFFSET = 8.7f;

        private const string UNTAGGED = "Untagged";
        public const string BLUE_PORTAL_TAG = "Blue Portal";
        public const string ORANGE_PORTAL_TAG = "Orange Portal";

        public const string SLICE_CENTER = "sliceCentre";
        public const string SLICE_NORMAL = "sliceNormal";
        public const string SLICE_OFFSET_DST = "sliceOffsetDst";

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

                Transform travellerTransform = traveller.transform;
                var linkWorldMatrix = linkedPortal.transform.localToWorldMatrix;
                var localMatrix = transform.worldToLocalMatrix;
                var travellerWorldMatrix = travellerTransform.localToWorldMatrix;

                var matrix = linkWorldMatrix * localMatrix * travellerWorldMatrix;

                Vector3 offsetFromPortal = travellerTransform.position - transform.position;

                var dot = Vector3.Dot(offsetFromPortal, transform.forward);
                int portalSide = Math.Sign(dot);

                var prevDot = Vector3.Dot(traveller.previousOffsetFromPortal, transform.forward);
                int portalSideOld = Math.Sign(prevDot);

                if (portalSide != portalSideOld) {
                    var positionOld = travellerTransform.position;
                    var rotOld = travellerTransform.rotation;

                    var pos = matrix.GetColumn(LAST_MATRIX_COLUMN);
                    var rot = matrix.rotation;

                    traveller.Teleport(transform, linkedPortal.transform, pos, rot);
                    traveller.graphicsClone.transform.SetPositionAndRotation(positionOld, rotOld);
                    linkedPortal.ProcessTravellerEnterPortal(traveller);
                    trackedTravellers.RemoveAt(i);
                    i--;

                } else {
                    var pos = matrix.GetColumn(LAST_MATRIX_COLUMN);
                    var rot = matrix.rotation;
                    traveller.graphicsClone.transform.SetPositionAndRotation(pos, rot);
                    traveller.previousOffsetFromPortal = offsetFromPortal;
                }
            }
        }

        public void PortalRenderer() {
            foreach (var traveller in trackedTravellers) {
                UpdateSliceParams(traveller);
            }
        }

        public void PostPortalRender() {
            foreach (var traveller in trackedTravellers) {
                UpdateSliceParams(traveller);
            }
        }


        private void UpdateSliceParams(PortalTraveller traveller) {
            int side = GetSideOfPortal(traveller.transform.position);
            Vector3 sliceNormal = transform.forward * -side;
            Vector3 cloneSliceNormal = linkedPortal.transform.forward * side;

            Vector3 slicePos = transform.position;
            Vector3 cloneSlicePos = linkedPortal.transform.position;

            float sliceOffsetDst = 0;
            float cloneSliceOffsetDst = 0;

            for (int i = 0; i < traveller.originalMaterials.Length; i++) {
                traveller.originalMaterials[i].SetVector(SLICE_CENTER, slicePos);
                traveller.originalMaterials[i].SetVector(SLICE_NORMAL, sliceNormal);
                traveller.originalMaterials[i].SetFloat(SLICE_OFFSET_DST, sliceOffsetDst);

                traveller.cloneMaterials[i].SetVector(SLICE_CENTER, cloneSlicePos);
                traveller.cloneMaterials[i].SetVector(SLICE_NORMAL, cloneSliceNormal);
                traveller.cloneMaterials[i].SetFloat(SLICE_OFFSET_DST, cloneSliceOffsetDst);

            }

        }

        private void ProcessTravellerEnterPortal(PortalTraveller traveller) {
            if (!trackedTravellers.Contains(traveller)) {
                traveller.EnterPortalThreshold();
                var offset = traveller.transform.position - transform.position;
                traveller.previousOffsetFromPortal = offset;
                trackedTravellers.Add(traveller);
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
            var arrow = traveller.GetComponent<Arrow>();

            if (arrow != null) {
                arrow.isTeleporting = true;
            }

            ProcessTravellerEnterPortal(traveller);
        }

        private void OnTriggerExit(Collider other) {
            if (!isReady) {
                return;
            }

            var traveller = other.GetComponent<PortalTraveller>();
            if (traveller && trackedTravellers.Contains(traveller)) {
                traveller.ExitPortalThreshold();
                trackedTravellers.Remove(traveller);
            }
            var arrow = other.GetComponent<Arrow>();

            if (arrow != null) {
                arrow.isTeleporting = false;
            }
        }

        private int GetSideOfPortal(Vector3 pos) {
            return Math.Sign(Vector3.Dot(pos - transform.position, transform.forward));
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


        }

        public void Close() {
            GetComponent<Collider>().enabled = false;
            tag = UNTAGGED;
            GetComponent<Animator>().SetTrigger(CLOSE_PORTAL_TRIGGER);
            Destroy(gameObject, CLOSE_ANIMATION_TIME);

            if (cameraRenderer == null) {
                Debug.LogError("Add Portal camera renderer");
                return;
            }
            cameraRenderer.portals.Remove(this);
        }
    }
}
