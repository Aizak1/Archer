using System;
using UnityEngine;

namespace Archer.Specs.LevelSpec {
    [Serializable]
    public struct LevelTaskSpec {
        public LevelTaskSpec(int primaryTargetCount, int secondaryTargetCount,
            int arrowToCompleteCount, int timeToComplete) {
            this.primaryTargetCount = primaryTargetCount;
            this.secondaryTargetCount = secondaryTargetCount;
            this.arrowToCompleteCount = arrowToCompleteCount;
            this.timeToComplete = timeToComplete;
        }

        [SerializeField] private int primaryTargetCount;
        public int PrimaryTargetCount => primaryTargetCount;

        [SerializeField] private int secondaryTargetCount;
        public int SecondaryTargetCount => secondaryTargetCount;

        [SerializeField] private int arrowToCompleteCount;
        public int ArrowToCompleteCount => arrowToCompleteCount;

        [SerializeField] private int timeToComplete;
        public int TimeToComplete => timeToComplete;
    }
}