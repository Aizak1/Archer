using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Archer.Controlls.LevelSystem;


namespace Archer.Controlls.Systems.Persistance {
    public class SceneControler : MonoBehaviour {
        [SerializeField] private List<string> levelSceneNameList;

        [SerializeField] private string UIstarnMenuSceneName;
        [SerializeField] private string registrationSceneName;
        [SerializeField] private string mainMenuSceneName;

        [SerializeField] private LoadSceneGroup startMenu;

        public void TryLoadStartScene() {
            StartCoroutine(LoadSceneGroup(startMenu));
        }

        private IEnumerator LoadSceneGroup(LoadSceneGroup group) {
            foreach (var sceneName in group.SceneNames) {
                if (SceneManager.GetSceneByName(sceneName).isLoaded) {
                    var scene = SceneManager.GetSceneByName(sceneName);
                    if (TryFindFirstOnScene(scene, out LevelSetup levelSetup)
                        && TryFindFirstOnScene(scene, out LevelUISetup levelUISetup)) {
                        levelSetup.OnLoad();
                    }
                    Debug.Log($"{sceneName} already Loaded");
                } else {
                    var load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                    yield return load;
                    var scene = SceneManager.GetSceneByName(sceneName);
                    if (TryFindFirstOnScene(scene, out LevelSetup levelSetup)
                        && TryFindFirstOnScene(scene, out LevelUISetup levelUISetup)) {
                        levelSetup.OnLoad();
                    }
                }
            }
        }

       // private void 

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

    [Serializable]
    public struct LoadSceneGroup {
        public LoadSceneGroup(string name, List<string> sceneNames) { 
            this.name = name;
            this.sceneNames = sceneNames;
        }

        [SerializeField] private List<string> sceneNames;
        public IEnumerable<string> SceneNames => sceneNames;

        [SerializeField] private string name;
        public string Name => name;
    }
}
