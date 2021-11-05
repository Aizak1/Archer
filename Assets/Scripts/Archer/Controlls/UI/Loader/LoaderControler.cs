using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Archer.Controlls.Systems.Persistance;
using Archer.Types.Levels;
using Archer.Extension.Scenes;

namespace Archer.Controlls.UI {
    public class LoaderControler : MonoBehaviour {
        [SerializeField] private Camera loaderCamera;

        private SceneControler sceneControler;

        private void Start() {
            var scene = SceneManager.GetSceneByName(Levels.MainControl.ToString());
            if (scene.isLoaded) {
                sceneControler = scene.GetComponent<SceneControler>();
                sceneControler?.LoaderNotify(this);
            }
        }

        public void ToggleVisibility(bool isVisible) {
            loaderCamera.enabled = isVisible;
            gameObject.SetActive(isVisible);
        }
    }
}
