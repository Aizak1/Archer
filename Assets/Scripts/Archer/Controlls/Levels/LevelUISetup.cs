using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Archer.Controlls.Systems.Persistance;
using Archer;

namespace Archer.Controlls.LevelSystem {
    public class LevelUISetup : MonoBehaviour {
        private LevelSetup levelSetup;
        private GameControler gameControler;


        public void Setup(GameControler gameControler, LevelSetup levelSetup) {
            this.gameControler = gameControler;
            this.levelSetup = levelSetup;
        }









    }
}
