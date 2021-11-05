using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Archer.Controlls.LevelSystem;
using Archer.Controlls.UI;
using Archer.Types.Levels;
using System.Linq;

namespace Archer.Controlls.Systems.Persistance {
    public class SceneControler : MonoBehaviour {
        [SerializeField] private List<string> levelSceneNameList;
        [SerializeField] private string loaderLevelName;

        [SerializeField] private string UIstarnMenuSceneName;
        [SerializeField] private string registrationSceneName;
        [SerializeField] private string mainMenuSceneName;

        [SerializeField] private LoadSceneGroup startMenu;
        [SerializeField] private LoadSceneGroup levelMenu;

        private LoaderControler loaderControler;
        private List<Scene> tracableLoadedScenes;

        private void Start() {
            tracableLoadedScenes = new List<Scene>();
        }

        public void LoaderNotify(LoaderControler loaderControler) {
            this.loaderControler = loaderControler;
        }

        public void TryLoadStartScene() {
            StartCoroutine(LoadSceneGroup(startMenu));
        }
        public void TryLoadLevelMenu() {
            StartCoroutine(LoadSceneGroup(levelMenu));
        }

        /*
        private IEnumerator LoadSceneGroup(LoadSceneGroup group, bool reload = false) {
            foreach (var sceneName in group.SceneNames) {
                if (SceneManager.GetSceneByName(sceneName).isLoaded) {
                    var scene = SceneManager.GetSceneByName(sceneName);
                    Debug.Log($"{sceneName} already Loaded");
                } else {
                    var load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                    yield return load;
                }
            }
        }
        */

        private IEnumerator LoadSceneGroup(LoadSceneGroup group) {
            loaderControler?.ToggleVisibility(true);
            var count = tracableLoadedScenes.Count;
            var asyncOperationList = new List<AsyncOperation>();

            foreach (var scene in tracableLoadedScenes) {
                asyncOperationList.Add(SceneManager.UnloadSceneAsync(scene.name));
            }

            foreach (var sceneName in group.SceneNames) {
                asyncOperationList.Add(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive));
            }

            while (!asyncOperationList.All(asyncOperation => asyncOperation.isDone)) {
                yield return new WaitForEndOfFrame();
            }

            var localLoadedSceneList = new List<Scene>();
            foreach (var sceneName in group.SceneNames) {
                var scene = SceneManager.GetSceneByName(sceneName);
                if (scene.isLoaded)
                    localLoadedSceneList.Add(scene);
            }

            tracableLoadedScenes = localLoadedSceneList;
            loaderControler?.ToggleVisibility(false);
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
