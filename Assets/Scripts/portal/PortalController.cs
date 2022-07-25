using System;
using UnityEngine;

namespace portal {
    public class PortalController : MonoBehaviour
    {

        private static PortalController _instance;

        public static PortalController Instance => _instance;
        
      
        [SerializeField] private PortalCameraRenderer _cameraRenderer;

       
        [SerializeField] private PortalSpawnObject _orangePortalPrefab;
       
        [SerializeField] private PortalSpawnObject _bluePortalPrefab;

        private Portal _orangePortal;
        private Portal _bluePortal;

        public bool _isBlue;

        private void Awake()
        {
            _instance = this;
        }

        public void CreatePortal(RaycastHit hit) {
            Vector3 pos = hit.point;
            Vector3 normal = hit.normal;
            normal.x = 0;
            var rot = Quaternion.LookRotation(normal);
            PortalSpawnObject portalSpawnObject;

            var parent = new GameObject();
            parent.transform.position = hit.collider.transform.position;
            parent.transform.rotation = hit.collider.transform.rotation;
            parent.transform.parent = hit.collider.transform;

            if (_isBlue) {

                if (_bluePortal) {
                    _bluePortal.Close();
                }
                portalSpawnObject = Instantiate(_bluePortalPrefab, pos, rot, parent.transform);
                _bluePortal = portalSpawnObject.PortalToSpawn;
                _bluePortal.Open(_cameraRenderer, _orangePortal);


            } else {

                if (_orangePortal) {
                    _orangePortal.Close();
                }
                portalSpawnObject = Instantiate(_orangePortalPrefab, pos, rot, parent.transform);
                _orangePortal = portalSpawnObject.PortalToSpawn;
                _orangePortal.Open(_cameraRenderer, _bluePortal);
            }

            Vector3 offset = portalSpawnObject.transform.forward.normalized / Portal.PORTAL_SPAWN_OFFSET;
            offset.x = 0;
            portalSpawnObject.transform.position += offset;
            
            _isBlue = !_isBlue;
        }

        private void OnDestroy()
        {
            _instance = null;
        }
    }
}
