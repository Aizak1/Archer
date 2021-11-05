using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Archer.Controlls.Systems.Persistance;
using Archer.Controlls.LevelSystem;

namespace Archer {
    public class GameControler : MonoBehaviour {
        [SerializeField] private SceneControler sceneControler;
        [SerializeField] private DataOperator dataOperat;

        private void Start() {
            dataOperat.LoadGame();
            sceneControler.TryLoadStartScene();
        }

        public void InitLevelSetup(
            IEnumerator<LevelSetup> levelSetupIEnum, IEnumerator<LevelUISetup> levelUISetupIEnum) {
        }

    }
}
