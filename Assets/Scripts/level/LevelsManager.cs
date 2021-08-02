using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace level {
    public class LevelsManager : MonoBehaviour {

        [SerializeField]
        private Button[] levelButtons;

        public const string LEVEL_AT = "levelAt";
        public const string STARTS_AT_LEVELS = "starsAtLevel";

        private void Start() {

            int levelAt = PlayerPrefs.GetInt(LEVEL_AT, 1);
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

    }
}

