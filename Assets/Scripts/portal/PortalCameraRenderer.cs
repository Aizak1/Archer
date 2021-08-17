using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace portal {
    public class PortalCameraRenderer : MonoBehaviour {
        public List<Portal> portals;

        private void Update() {
            for (int i = 0; i < portals.Count; i++) {
                portals[i].PortalRenderer();
            }
        }

    }
}