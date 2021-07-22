using bow;
using arrow;
using UnityEngine;
using UnityEngine.UI;
using level;

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

        private void Update() {
            enemyCountText.text = levelController.PeelEnemiesCount().ToString();
        }

        public void SwitchArrowTypeButton() {
            ArrowType currentArrowType = bowController.arrowTypeToInstantiate;
            int currentTypeIndex = resource.arrowTypeToCount[currentArrowType];
            int nextTypeIndex = currentTypeIndex + 1;

            if(nextTypeIndex == resource.arrowTypeToCount.Count) {
                nextTypeIndex = 0;
            }

            ArrowType nextType = resource.countToArrowType[nextTypeIndex];
            bowController.arrowTypeToInstantiate = nextType;
        }
    }
}

