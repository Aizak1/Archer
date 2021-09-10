using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ui;
using level;
using DG.Tweening;

namespace arrow {
    public class BonusArrowGiver : MonoBehaviour {
        [SerializeField]
        private GameObject bonusArrowPrefab;

        [SerializeField]
        private int remainTargetsForBonus;

        [SerializeField]
        private UiController uiController;
        [SerializeField]
        private ArrowResource arrowResource;
        [SerializeField]
        private LevelController levelController;

        [SerializeField]
        private GameObject bonusUI;

        [SerializeField]
        private GameObject[] miniUiArrowsToActivate;

        private void Update() {
            if (levelController.PeelTargetsCount() > remainTargetsForBonus) {
                return;
            }

            var arrowType = bonusArrowPrefab.GetComponentInChildren<Arrow>().arrowType;

            arrowResource.arrowPrefabs.Add(arrowType, bonusArrowPrefab);
            arrowResource.countToArrowType.Add(arrowResource.countToArrowType.Count, arrowType);
            arrowResource.arrowTypeToCount.Add(arrowType, arrowResource.arrowTypeToCount.Count);

            bonusUI.transform.DOScale(1.5f, 1f).SetLoops(2, LoopType.Yoyo);

            uiController.SwitchLastArrow();

            for (int i = 0; i < miniUiArrowsToActivate.Length; i++) {
                miniUiArrowsToActivate[i].SetActive(true);
            }

            enabled = false;
        }
    }
}
