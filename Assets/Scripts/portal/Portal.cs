using UnityEngine;

namespace portal {

    public class Portal : MonoBehaviour {
        public bool isOrange;

        public Portal linkedPortal;

        public bool isReady = false;

        private const string OPEN_PORTAL_TRIGGER = "Open";
        private const string CLOSE_PORTAL_TRIGGER = "Close";

        private const float CLOSE_ANIMATION_TIME = 0.5f;
        private const int LAST_MATRIX_COLUMN = 3;

        private const string UNTAGGED = "Untagged";
        public const string BLUE_PORTAL_TAG = "Blue Portal";
        public const string ORANGE_PORTAL_TAG = "Orange Portal";

        public void MakeTeleport(GameObject objectToTeleport) {
            if (!isReady) {
                return;
            }
            var linkedWorldMatrix = linkedPortal.transform.localToWorldMatrix;
            var localMatrix = transform.worldToLocalMatrix;
            var travellerWorldMatrix = objectToTeleport.transform.localToWorldMatrix;
            var matrix = linkedWorldMatrix * localMatrix * travellerWorldMatrix;

            var pos = matrix.GetColumn(LAST_MATRIX_COLUMN);
            Teleport(objectToTeleport, pos, matrix.rotation);
        }

        public void Teleport(GameObject objectToTeleport, Vector3 pos, Quaternion rot) {
            var rigidbody = objectToTeleport.GetComponent<Rigidbody>();
            if (rigidbody == null) {
                return;
            }
            objectToTeleport.transform.position = pos;
            objectToTeleport.transform.rotation = rot;
            var inverseVel = transform.InverseTransformVector(rigidbody.velocity);
            var vel = linkedPortal.transform.TransformVector(inverseVel);
            rigidbody.velocity = vel;

            var inverseAngularVel = transform.InverseTransformVector(rigidbody.angularVelocity);
            var angVel = linkedPortal.transform.TransformVector(inverseAngularVel);
            rigidbody.angularVelocity = angVel;
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

        }

        public void Close() {
            tag = UNTAGGED;
            GetComponent<Animator>().SetTrigger(CLOSE_PORTAL_TRIGGER);
            Destroy(gameObject, CLOSE_ANIMATION_TIME);
        }
    }
}
