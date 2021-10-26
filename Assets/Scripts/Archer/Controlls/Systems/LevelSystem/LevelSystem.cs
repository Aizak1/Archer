using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Archer.Controlls.IHitableAction;

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
            levelTargetsList = targetContainer.GetComponentsInChildren<ArrowHitableLevelTarget>().ToList();
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
    
    public struct LevelTaskSpec {
        public LevelTaskSpec(int primaryTargetCount, int secondaryTargetCount,
            int arrowToCompleteCount, int timeToComplete) {
            this.primaryTargetCount = primaryTargetCount;
            this.secondaryTargetCount = secondaryTargetCount;
            this.arrowToCompleteCount = arrowToCompleteCount;
            this.timeToComplete = timeToComplete;
        }

        private int primaryTargetCount;
        public int PrimaryTargetCount => primaryTargetCount;

        private int secondaryTargetCount;
        public int SecondaryTargetCount => secondaryTargetCount;

        private int arrowToCompleteCount;
        public int ArrowToCompleteCount => arrowToCompleteCount;

        private int timeToComplete;
        public int TimeToComplete => timeToComplete;
    }





    /*
    public interface IResourceWatcherServece {

        void Init(LevelSpec spec);
        void StartSesion(Action onFinishCallback);
    }
   
    public class TimeWatcherServece : MonoBehaviour, IResourceWatcherServece {
        public void Init(LevelSpec spec) { }

        public void StartSesion(Action onFinishCallback) { }

        private void Reset() { }

    }

    public class ArrowWatcherServece : MonoBehaviour, IResourceWatcherServece {
        public void Init(LevelSpec spec) { }

        public void StartSesion(Action onFinishCallback) { }

        private void Reset() { }
    }

    public class PrimaryTargetWatcherServece : MonoBehaviour, IResourceWatcherServece
    {
        public void Init(LevelSpec spec) { }

        public void StartSesion(Action onFinishCallback) { }

        private void Reset() { }
    }

    public class SecondaryTargetWatcherServece : MonoBehaviour, IResourceWatcherServece
    {
        public void Init(LevelSpec spec) { }

        public void StartSesion(Action onFinishCallback) { }

        private void Reset() { }
    }
    */
}
