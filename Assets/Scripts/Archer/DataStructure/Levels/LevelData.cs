using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Archer.Specs.LevelSpec;

namespace Archer.DataStructure.Levels {
    [Serializable]
    public struct LevelDescriptor {
        public LevelDescriptor(int id, string selector, List<int> scoreLowerBounds,
            LevelTaskSpec levelSpec, List<GameObject> infoPushs) {
            this.id = id;
            this.selector = selector;
            this.scoreLowerBounds = scoreLowerBounds;
            this.levelSpec = levelSpec;
            this.infoPushs = infoPushs;
        }

        [SerializeField] private int id;
        public int Id => id;

        [SerializeField] private string selector;
        public string Selectord => selector;

        [SerializeField] private List<int> scoreLowerBounds;
        public IEnumerable<int> ScoreLowerBounds => scoreLowerBounds;

        [SerializeField] private LevelTaskSpec levelSpec;
        public LevelTaskSpec LevelSpec => levelSpec;

        [SerializeField] private List<GameObject> infoPushs;
        public IEnumerable<GameObject> InfoPushs => infoPushs;
    }

    [Serializable]
    public struct LevelResult {
        public LevelResult(int id, int score) {
            this.id = id;
            this.score = score;
        }

        [SerializeField] private int id;
        public int Id => id;

        [SerializeField] private int score;
        public int Score => score;
    }
}
