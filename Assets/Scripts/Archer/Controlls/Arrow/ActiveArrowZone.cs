using Archer.Controlls.ArrowControlls;
using UnityEngine;

namespace Archer.ActiveArrowZoneController {
    public class ActiveArrowZone : MonoBehaviour
    {
        private void OnTriggerExit(Collider other) {
            if (other.TryGetComponent(out ArrowController arrowController)) {
                Destroy(arrowController.gameObject);
            }
        }
    }
}
