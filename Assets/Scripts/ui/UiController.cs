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
        private BowController bowController;
        [SerializeField]
        private ArrowResource resource;

        [SerializeField]
        private LevelController levelController;
        [SerializeField]
        private Text enemyCountText;

        [SerializeField]
        private TextMeshProUGUI arrowTypeText;
        [SerializeField]
        private RawImage arrowImage;
        [SerializeField]
        private Texture[] arrowImageTextures;

        [SerializeField]
        private GameObject[] quiverGroups;

        private  Dictionary<ArrowType, Texture> arrowTypeToTexture
          = new Dictionary<ArrowType, Texture>();

        private Dictionary<ArrowType, GameObject> arrowTypeToQuiverGroup
            = new Dictionary<ArrowType, GameObject>();




        [SerializeField]
        private SceneLoader sceneLoader;

        private void Start() {
            if(arrowTypeText == null || arrowImage == null) {
                return;
            }

            if(arrowImageTextures.Length < resource.countToArrowType.Count) {
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
        }

        private void Update() {
            if (enemyCountText != null && levelController != null) {
                enemyCountText.text = levelController.PeelEnemiesCount().ToString();
            }
        }

        public void LoadNextLevelButton() {
            sceneLoader.LoadNextLevel();
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

        }
    }
}

