using UnityEngine;

namespace Archer.Controlls.IHitableAction {
    public class ArrowObjectEnable : MonoBehaviour, IHitable {
        [SerializeField] private GameObject enabledObject;

        public void HitAction()
        {
            enabledObject.SetActive(true);
        }
    }
}
