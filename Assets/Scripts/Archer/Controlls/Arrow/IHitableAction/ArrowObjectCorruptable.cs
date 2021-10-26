using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Archer.Controlls.Systems.DynamicOclusion;

namespace Archer.Controlls.IHitableAction {
    public class ArrowObjectCorruptable : MonoBehaviour, IHitable {
        public bool IsAvalible => isAvalible;
        public bool IsCorrupted => isCorrpted;
        public bool IsStateChange => isStateChange;
        public HitableAccessFlag Type => type;

        private bool isStateChange;
        private bool isCorrpted;
        private bool isAvalible;
        private HitableAccessFlag type = HitableAccessFlag.corrupt;

        public void HitAction() {
            isCorrpted = true;
        }

        public void SetChangeState() {
            isStateChange = true;
        }

        public bool TrySetIsAvalible() {
            if (!isCorrpted) {
                isAvalible = true;
                return true;
            }
            return false;
        }

        public bool TryToLaunch() {
            if (isCorrpted || !IsAvalible)
                return false;
            gameObject.SetActive(true);
            isAvalible = false;
            return true;
        }
    }
}
