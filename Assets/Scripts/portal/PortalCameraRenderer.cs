using System.Collections.Generic;
using UnityEngine;

namespace portal {
    public class PortalCameraRenderer : MonoBehaviour {
        public List<Portal> portals = new List<Portal>();

        private void Update() {
            for (int i = 0; i < portals.Count; i++) {
                portals[i].PortalRenderer();
            }
        }
    }
}