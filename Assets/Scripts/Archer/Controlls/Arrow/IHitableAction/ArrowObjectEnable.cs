using UnityEngine;

namespace Archer.Controlls.IHitableAction {
    public class ArrowObjectEnable : MonoBehaviour, IHitable {
        [SerializeField] private GameObject enabledObject;

        public HitableAccessFlag Type => type;
        private HitableAccessFlag type = HitableAccessFlag.enableObject;

        public void HitAction()
        {
            enabledObject.SetActive(true);
        }
    }
}
