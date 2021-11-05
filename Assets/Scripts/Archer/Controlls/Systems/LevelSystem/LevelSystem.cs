using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Archer.Controlls.IHitableAction;
using Archer.Specs.LevelSpec;

namespace Archer.Controlls.Systems.LevelSystem {
    public class LevelControler : MonoBehaviour {
        [SerializeField] private Transform targetContainer;

        private List<ArrowHitableLevelTarget> levelTargetsList;
        private LevelTaskSpec levelSpec;

        private void Start() {

        }

        private void Init(LevelTaskSpec levelSpec) {
            ResetLevelSystem();
            this.levelSpec = levelSpec;
            levelTargetsList = 
                targetContainer.GetComponentsInChildren<ArrowHitableLevelTarget>().ToList();
        }

        private void OnTargetHit(ArrowHitableLevelTarget levelTarget) {
            var type = levelTarget.TargetType;

            if (type == LevelTargetType.primary) {

            } else {

            }
            var islevelComplete = IsLevelObjectiveComplete();
        }

        private bool IsLevelObjectiveComplete() {
            return false;
        }

        private void ToggleSubscribtion(bool subscribe) {
        }

        private void ResetLevelSystem() {
        }
    }
}
