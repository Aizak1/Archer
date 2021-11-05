using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Archer;

namespace Archer.Controlls.LevelSystem {
    public class LevelSetup : MonoBehaviour {
        private LevelSetup levelSetup;
        private GameControler gameControler;

        public void Setup(GameControler gameControler, LevelSetup levelSetup) {
            this.gameControler = gameControler;
            this.levelSetup = levelSetup;
        }

        public void OnLoad() {
            Debug.Log("OnLoad");
        }

        public void OnDestroy() {
            Debug.Log("destroy onload");
        }
    }
}
