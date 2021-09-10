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
        private BowController bowController;
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

        private  Dictionary<int, Texture> arrowTypeToTexture = new Dictionary<int, Texture>();

        private Dictionary<int, GameObject> arrowTypeToQuiverGroup
            = new Dictionary<int, GameObject>();

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

            if (arrowImageTextures.Length < resource.countToArrowType.Count) {
                Debug.LogError("Lack of Images");
                return;
            }

            if (quiverGroups.Length < resource.countToArrowType.Count) {
                Debug.LogError("Lack of Quiver groups");
                return;
            }

            for (int i = 0; i < arrowImageTextures.Length; i++) {
                arrowTypeToTexture.Add(i, arrowImageTextures[i]);

                quiverGroups[i].SetActive(false);
                arrowTypeToQuiverGroup.Add(i, quiverGroups[i]);
            }

            arrowTypeText.text = bowController.arrowTypeToInstantiate.ToString();
            var currentType = bowController.arrowTypeToInstantiate;
            arrowImage.texture = arrowTypeToTexture[0];
            arrowTypeToQuiverGroup[0].SetActive(true);
            int currentTypeIndex = resource.arrowTypeToCount[currentType];
            trajectoryShower.SetSettings(currentTypeIndex);
        }

        private void Update() {
            if (enemyCountText != null && levelController != null) {
                enemyCountText.text = "";
                enemyCountText.text += levelController.PeelTargetsCount();

                spendedArrowsText.text = bowController.shotsCount + " / " + arrowsForStar;
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

        public void SwitchArrowTypeButton() {
            ArrowType currentArrowType = bowController.arrowTypeToInstantiate;
            int currentTypeIndex = resource.arrowTypeToCount[currentArrowType];
            arrowTypeToQuiverGroup[currentTypeIndex].SetActive(false);
            int nextTypeIndex = currentTypeIndex + 1;

            if (nextTypeIndex == resource.arrowTypeToCount.Count) {
                nextTypeIndex = 0;
            }

            ArrowType nextType = resource.countToArrowType[nextTypeIndex];

            bowController.arrowTypeToInstantiate = nextType;
            arrowTypeText.text = nextType.ToString();
            arrowImage.texture = arrowTypeToTexture[nextTypeIndex];
            arrowTypeToQuiverGroup[nextTypeIndex].SetActive(true);

            trajectoryShower.SetSettings(nextTypeIndex);
        }

        public void SwitchLastArrow() {
            ArrowType currentArrowType = bowController.arrowTypeToInstantiate;
            int currentTypeIndex = resource.arrowTypeToCount[currentArrowType];
            arrowTypeToQuiverGroup[currentTypeIndex].SetActive(false);

            int nextTypeIndex = resource.arrowTypeToCount.Count - 1;

            ArrowType nextType = resource.countToArrowType[nextTypeIndex];

            bowController.arrowTypeToInstantiate = nextType;
            arrowTypeText.text = nextType.ToString();
            arrowImage.texture = arrowTypeToTexture[nextTypeIndex];
            arrowTypeToQuiverGroup[nextTypeIndex].SetActive(true);

            trajectoryShower.SetSettings(nextTypeIndex);
        }
    }
}