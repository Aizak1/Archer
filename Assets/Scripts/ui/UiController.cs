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
        private TrajectorySettings[] arrowTrajectorySettings;
        [SerializeField]
        private GameObject[] quiverGroups;

        private  Dictionary<ArrowType, Texture> arrowTypeToTexture
          = new Dictionary<ArrowType, Texture>();

        private Dictionary<ArrowType, GameObject> arrowTypeToQuiverGroup
            = new Dictionary<ArrowType, GameObject>();

        private string arrowsForStar;

        [SerializeField]
        private SceneLoader sceneLoader;

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

            for (int i = 0; i < resource.countToArrowType.Count; i++) {
                arrowTypeToTexture.Add(resource.countToArrowType[i], arrowImageTextures[i]);

                quiverGroups[i].SetActive(false);
                arrowTypeToQuiverGroup.Add(resource.countToArrowType[i], quiverGroups[i]);
            }

            arrowTypeText.text = bowController.arrowTypeToInstantiate.ToString();
            var currentType = bowController.arrowTypeToInstantiate;
            arrowImage.texture = arrowTypeToTexture[currentType];
            arrowTypeToQuiverGroup[currentType].SetActive(true);
            int currentTypeIndex = resource.arrowTypeToCount[currentType];
            trajectoryShower.SetSettings(arrowTrajectorySettings[currentTypeIndex]);
        }

        private void Update() {
            if (enemyCountText != null && levelController != null) {
                enemyCountText.text = levelController.PeelTargetsCount().ToString();

                string currentArrows = bowController.shotsCount.ToString();

                spendedArrowsText.text = currentArrows + " / " + arrowsForStar;
                var targetTime = levelController.PeelStarTime();
                var currentTime = levelController.PeelTimeSinceStart();
                var timeRemain = Mathf.RoundToInt(targetTime - currentTime);

                if (timeRemain <= 0) {
                    timeText.text = "0";
                    timeText.color = Color.red;
                } else {
                    timeText.text = timeRemain.ToString();
                }
            }
        }

        public void SwitchArrowTypeButton() {
            ArrowType currentArrowType = bowController.arrowTypeToInstantiate;
            arrowTypeToQuiverGroup[currentArrowType].SetActive(false);
            int currentTypeIndex = resource.arrowTypeToCount[currentArrowType];
            int nextTypeIndex = currentTypeIndex + 1;

            if (nextTypeIndex == resource.arrowTypeToCount.Count) {
                nextTypeIndex = 0;
            }

            ArrowType nextType = resource.countToArrowType[nextTypeIndex];
            bowController.arrowTypeToInstantiate = nextType;
            arrowTypeText.text = nextType.ToString();
            arrowImage.texture = arrowTypeToTexture[nextType];
            arrowTypeToQuiverGroup[nextType].SetActive(true);
            trajectoryShower.SetSettings(arrowTrajectorySettings[nextTypeIndex]);
        }
    }
}