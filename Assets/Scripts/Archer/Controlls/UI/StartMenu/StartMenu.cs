using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Archer.Controlls.UI {
    public class StartMenu : MonoBehaviour {

        private GameControler gameControler;
        private string mainControl = "MainControl";

        private void Start() {
            var scene = SceneManager.GetSceneByName(mainControl);
            if (TryFindFirstOnScene(scene, out GameControler result)) {
                gameControler = result;
            }
        }

        public void Quit() {

        }

        public void Play() {
            gameControler.LoadLevelMenu();
        }

        private bool TryFindFirstOnScene<T>(Scene scene, out T result) where T : MonoBehaviour {
            var firstLevelGOArray = scene.GetRootGameObjects();
            foreach (var topLevelGO in firstLevelGOArray) {
                var component = topLevelGO.GetComponentInChildren<T>();
                if (component != null) {
                    result = component;
                    return true;
                }
            }
            result = null;
            return false;
        }
    }

}
