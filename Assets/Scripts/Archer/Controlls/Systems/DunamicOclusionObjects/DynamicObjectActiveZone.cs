using UnityEngine;
using Archer.Controlls.IHitableAction;

namespace Archer.Controlls.Systems.DynamicOclusion {
    public class DynamicObjectActiveZone : MonoBehaviour {
        private void OnTriggerExit(Collider other) {
            if (other.TryGetComponent(out ArrowObjectCorruptable corruptable)) {
                if (corruptable.IsCorrupted)
                    Destroy(corruptable.gameObject);
            }
        }
    }
}
