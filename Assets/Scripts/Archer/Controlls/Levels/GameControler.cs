using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Archer.Controlls.Systems.Persistance;
using Archer.Controlls.LevelSystem;
using Archer.Controlls.Systems.InfoPopup;

namespace Archer {
    public class GameControler : MonoBehaviour {
        [SerializeField] private InfoPopupControler infoPopup;
        [SerializeField] private SceneControler sceneControler;
        [SerializeField] private DataOperator dataOperat;

        public void LoadLevelMenu() {

        }

        public void LoadStartMenu() {

        }

        public void AddPopup(GameObject popup) {

        }

        private void Start() {
            //dataOperat.LoadGame();
            //sceneControler.TryLoadStartScene();
        }

        [ContextMenu("1")]
        private void QQQ() {
            sceneControler.TryLoadLevelMenu();
        }

        [ContextMenu("2")]
        private void RRR() {
            sceneControler.TryLoadStartScene();
        }

        public void InitLevelSetup(
            IEnumerator<LevelSetup> levelSetupIEnum, IEnumerator<LevelUISetup> levelUISetupIEnum) {
        }
    }
}
