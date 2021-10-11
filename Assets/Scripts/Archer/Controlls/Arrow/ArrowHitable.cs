using System.Collections.Generic;
using UnityEngine;
using Archer.Controlls.IHitableAction;

namespace Archer.Controlls.ArrowHitableControlls {
    [RequireComponent(typeof(Collider))]
    public class ArrowHitable : MonoBehaviour {
        [Header("Params")]
        [SerializeField] private float hardnes;
        [SerializeField] private float bounce;
        [Header("Options")]
        [SerializeField] private bool isEndless;
        [Header("IHitableActions")]
        [SerializeField] private List<GameObject> ihitableGOList = new List<GameObject>();

        public float Hardnes => hardnes;
        public float Bounce => bounce;
        public bool IsEndless => isEndless;
        
        private List<IHitable> iHitableActionsList;

        private void Start() {
            InitIhitable();
        }
        
        private void OnValidate() {
            ValidateIhitable();
        }

        public void PerformHit() {
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
