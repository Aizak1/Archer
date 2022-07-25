using bow;
using arrow;
using UnityEngine;
using UnityEngine.UI;
using level;
using TMPro;
using System.Collections.Generic;

namespace ui {
    public class UiController : MonoBehaviour {

        [SerializeField]
        private TrajectoryShower trajectoryShower;
        [SerializeField]
        private Bow _bow;
        [SerializeField]
        private ArrowResource resource;

        [SerializeField]
        private LevelController levelController;
        [SerializeField]
        private TextMeshProUGUI enemyCountText;

        [SerializeField]
        private TextMeshProUGUI arrowTypeText;
        [SerializeField]
        private RawImage arrowImage;

        [SerializeField]
        private TextMeshProUGUI spendedArrowsText;
        [SerializeField]
        private TextMeshProUGUI timeText;


        [SerializeField]
        private Texture[] arrowImageTextures;
        [SerializeField]
        private GameObject[] quiverGroups;
        

        private string arrowsForStar;

        [SerializeField]
        private SceneLoader sceneLoader;

        private float targetTime;
        private float currentTime;
        private int timeRemain;

        private void Start() {
            arrowsForStar = levelController.PeelCountOfArrowsForStar().ToString();
            if (arrowTypeText == null || arrowImage == null) {
                return;
            }
            //arrowTypeText.text = bowController.arrowToInstantiate.ToString();
            trajectoryShower.SetSettings(0);
            ShowEnemyCountText();
            levelController.OnTargetsDecrease.AddListener(ShowEnemyCountText);
        }

        private void Update() {
            if (enemyCountText != null && levelController != null) {
                spendedArrowsText.text = _bow.shotsCount + " / " + arrowsForStar;
                targetTime = levelController.PeelStarTime();
                currentTime = levelController.PeelTimeSinceStart();
                timeRemain = Mathf.RoundToInt(targetTime - currentTime);

                if (timeRemain <= 0) {
                    timeText.text = "0";
                    timeText.color = Color.red;
                } else {
                    timeText.text = "";
                    timeText.text += timeRemain;
                }
            }
        }

        private void ShowEnemyCountText()
        {
            enemyCountText.text = levelController.PeelTargetsCount().ToString();
        }

        public void SwitchArrowTypeButton() {
            //ArrowType currentArrowType = bowController.arrowToInstantiate;
            //arrowTypeToQuiverGroup[currentArrowType].SetActive(false);
            //int currentTypeIndex = resource.arrowTypeToCount[currentArrowType];
            //int nextTypeIndex = currentTypeIndex + 1;

            //if (nextTypeIndex == resource.arrowTypeToCount.Count) {
             //   nextTypeIndex = 0;
            //}

            //ArrowType nextType = resource.countToArrowType[nextTypeIndex];

            //bowController.arrowToInstantiate = nextType;
            //arrowTypeText.text = nextType.ToString();
            //arrowImage.texture = arrowTypeToTexture[nextType];
            //arrowTypeToQuiverGroup[nextType].SetActive(true);

            //trajectoryShower.SetSettings(nextTypeIndex);
        }
    }
}