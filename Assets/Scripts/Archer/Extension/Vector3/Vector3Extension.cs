using UnityEngine;

namespace Archer.Extension.Vector3Extension {
    public static class Vector3Extension
    {
        public static bool OnLine(this Vector3 point, Vector3 startPoint, Vector3 endPoint) {
            var ac = (point - startPoint).magnitude;
            var cb = (point - endPoint).magnitude;
            var ab = (startPoint - endPoint).magnitude;
            if (Mathf.Abs(ac + cb - ab) < 0.001f)
                return true;
            else
                return false;
        }
    }
}
