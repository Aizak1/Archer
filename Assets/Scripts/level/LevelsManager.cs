using UnityEngine;
using UnityEngine.UI;

namespace level {
    public class LevelsManager : MonoBehaviour {

        [SerializeField]
        private Button[] levelButtons;
        [SerializeField]
        private Page[] pages;

        private int currentPageNumber;

        public const string LEVEL_AT = "levelAt";
        public const string STARTS_AT_LEVELS = "starsAtLevel";

        private void Start() {
            int levelAt = PlayerPrefs.GetInt(LEVEL_AT, 1);
            int lastLevelPicked = PlayerPrefs.GetInt(SceneLoader.LAST_PICKED_LEVEL, 1);
            int countOfLevelsOnPage = 0;

            for (int i = 0; i < pages.Length; i++) {
                countOfLevelsOnPage += pages[i].pageButtons.Length;
                if(lastLevelPicked <= countOfLevelsOnPage) {
                    currentPageNumber = i;
                    break;
                } else {
                    continue;
                }
            }
            ShowPage();

            for (int i = levelAt; i < levelButtons.Length; i++) {
                levelButtons[i].interactable = false;
            }

            var starsAtLevels = PlayerPrefs.GetString(STARTS_AT_LEVELS,"");
            for (int i = 0; i < levelAt - 1; i++) {

                var starsObjects = levelButtons[i].GetComponentsInChildren<Star>();

                for (int j = 0; j < int.Parse(starsAtLevels[i].ToString()); j++) {
                    starsObjects[j].transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }

        public void ShowNextPage() {
            if(currentPageNumber + 1 >= pages.Length) {
                return;
            }

            foreach (var item in pages[currentPageNumber + 1].pageButtons) {
                item.gameObject.SetActive(true);
            }

            foreach (var item in pages[currentPageNumber].pageButtons) {
                item.gameObject.SetActive(false);
            }

            currentPageNumber++;
        }

        public void ShowPrevPage() {
            if(currentPageNumber - 1 < 0) {
                return;
            }

            foreach (var item in pages[currentPageNumber - 1].pageButtons) {
                item.gameObject.SetActive(true);
            }

            foreach (var item in pages[currentPageNumber].pageButtons) {
                item.gameObject.SetActive(false);
            }

            currentPageNumber--;
        }

        public void ShowPage() {
            foreach (var item in pages) {
                for (int i = 0; i < item.pageButtons.Length; i++) {
                    item.pageButtons[i].gameObject.SetActive(false);
                }
            }

            foreach (var item in pages[currentPageNumber].pageButtons) {
                item.gameObject.SetActive(true);
            }
        }
    }

    [System.Serializable]
    public class Page {
        public Button[] pageButtons;
    }
}