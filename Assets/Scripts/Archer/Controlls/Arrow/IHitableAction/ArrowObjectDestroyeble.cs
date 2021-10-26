using System.Collections.Generic;
using UnityEngine;

namespace Archer.Controlls.IHitableAction {
    public class ArrowObjectDestroyeble : MonoBehaviour, IHitable {

        [SerializeField] private List<Component> destroyComponentsList;
        [SerializeField] private List<GameObject> destroyGOList;

        public HitableAccessFlag Type => type;
        private HitableAccessFlag type = HitableAccessFlag.destroyObject;

        public void HitAction() {
            foreach (var component in destroyComponentsList) {
                Destroy(component);
            }
            foreach (var go in destroyGOList) {
                Destroy(go);
            }
        }
    }
}
