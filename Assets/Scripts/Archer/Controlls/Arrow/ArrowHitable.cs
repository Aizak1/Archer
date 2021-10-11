using System.Collections.Generic;
using UnityEngine;
using Archer.Controlls.IHitableAction;
using Archer.Controlls.ArrowControlls;

namespace Archer.Controlls.ArrowHitableControlls {
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(ArrowBalisticInteraction))]
    public class ArrowHitable : MonoBehaviour {
        [Header("IHitableActions")]
        [SerializeField] private List<GameObject> ihitableGOList = new List<GameObject>();

        private ArrowBalisticInteraction arrowBalisticInteraction;
        private List<IHitable> iHitableActionsList;

        private void Start() {
            arrowBalisticInteraction = GetComponent<ArrowBalisticInteraction>();
            InitIhitable();
        }
        
        private void OnValidate() {
            ValidateIhitable();
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

        private void ValidateIhitable() {
            var localIHitableList = new List<GameObject>();
            if (ihitableGOList != null && ihitableGOList.Count > 0) {
                foreach (var ihitable in ihitableGOList) {
                    if (ihitable.TryGetComponent<IHitable>(out _)) {
                        localIHitableList.Add(ihitable);
                    }
                }
            }
            ihitableGOList = localIHitableList;
        }

        private void InitIhitable() {
            var localIhitableList = new List<IHitable>();
            foreach (var ihitableGO in ihitableGOList) {
                if (ihitableGO.TryGetComponent(out IHitable ihitable)) {
                    localIhitableList.Add(ihitable);
                }
            }
            iHitableActionsList = localIhitableList;
        }
    }
}
