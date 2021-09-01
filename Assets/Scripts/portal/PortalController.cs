using UnityEngine;

namespace portal {
    public class PortalController : MonoBehaviour {
        [SerializeField]
        private PortalCameraRenderer cameraRenderer;

        [SerializeField]
        private GameObject orangePortalPrefab;
        [SerializeField]
        private GameObject bluePortalPrefab;

        private Portal orangePortal;
        private Portal bluePortal;

        public bool isBlue;

        public void CreatePortal(RaycastHit hit) {
            Vector3 pos = hit.point;
            Vector3 normal = hit.normal;
            normal.x = 0;
            var rot = Quaternion.LookRotation(normal);
            GameObject portal;

            var parent = new GameObject();
            parent.transform.position = hit.collider.gameObject.transform.position;
            parent.transform.rotation = hit.collider.gameObject.transform.rotation;
            parent.transform.parent = hit.collider.gameObject.transform;

            if (isBlue) {

                if (bluePortal) {
                    bluePortal.Close();
                }
                portal = Instantiate(bluePortalPrefab, pos, rot, parent.transform);
                bluePortal = portal.GetComponentInChildren<Portal>();
                bluePortal.Open(cameraRenderer, orangePortal);


            } else {

                if (orangePortal) {
                    orangePortal.Close();
                }
                portal = Instantiate(orangePortalPrefab, pos, rot, parent.transform);
                orangePortal = portal.GetComponentInChildren<Portal>();
                orangePortal.Open(cameraRenderer, bluePortal);
            }

            Vector3 offset = portal.transform.forward.normalized / Portal.PORTAL_SPAWN_OFFSET;
            offset.x = 0;
            portal.transform.position += offset;
        }
    }
}
