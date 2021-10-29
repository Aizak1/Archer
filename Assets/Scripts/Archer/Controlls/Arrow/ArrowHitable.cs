using System.Collections.Generic;
using Archer.Controlls.IHitableAction;
using Archer.Controlls.ArrowControlls;
using UnityEngine;

namespace Archer.Controlls.ArrowHitableControlls {
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(ArrowBalisticInteraction))]
    public class ArrowHitable : MonoBehaviour {
        [SerializeField] public HitableAccessFlag hitableAccessFlag;
        [Header("IHitableActions")]
        [SerializeField] private List<GameObject> ihitableGOList = new List<GameObject>();

        private ArrowBalisticInteraction arrowBalisticInteraction;
        private List<IHitable> iHitableActionsList;

        private void Start() {
            arrowBalisticInteraction = GetComponent<ArrowBalisticInteraction>();
            InitIHitable();
        }
        
        private void OnValidate() {
            ValidateIHitable();
        }

        public void PerformHit(ArrowController arrow) {
            arrowBalisticInteraction.PerformBalisticHit(arrow);
            PerformIHitable();
        }

        private void PerformIHitable() {
            foreach (var ihitable in iHitableActionsList) {
                ihitable.HitAction();
            }
        }

        private void ValidateIHitable() {
            var localIHitableList = new List<GameObject>();
            if (ihitableGOList != null && ihitableGOList.Count > 0) {
                foreach (var ihitableGo in ihitableGOList) {

                    var ihitabls = ihitableGo.GetComponents<IHitable>();
                    if (ihitabls.Length > 0) {
                        var isFind = false;
                        foreach (var ihitable in ihitabls) {
                            if (hitableAccessFlag.HasFlag(ihitable.Type)) {
                                isFind = true;
                                break;
                            }
                        }
                        if (isFind) {
                            localIHitableList.Add(ihitableGo);
                        }
                    }
                }
            }
            ihitableGOList = localIHitableList;
        }

        private void InitIHitable() {
            var localIhitableList = new List<IHitable>();
            foreach (var ihitableGO in ihitableGOList) {
                var ihitables = ihitableGO.GetComponents<IHitable>();
                foreach (var ihitable in ihitables)
                    if (hitableAccessFlag.HasFlag(ihitable.Type))
                        localIhitableList.Add(ihitable);
            }
            iHitableActionsList = localIhitableList;
        }
    }
}
