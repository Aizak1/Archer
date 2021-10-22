using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archer.Controlls.IHitableAction {
    public class ArrowHitableLevelTarget : MonoBehaviour, IHitable {
        [SerializeField] private LevelTargetType targetType;
        public HitableAccessFlag Type => type;
        public LevelTargetType TargetType => targetType;

        private HitableAccessFlag type = HitableAccessFlag.levelTarget;

        public Action<ArrowHitableLevelTarget> LevelTargetGitAction;

        public void HitAction() {
            LevelTargetGitAction?.Invoke(this);
        }
    }

    public enum LevelTargetType {
        primary,
        secondary,
    }
}
