using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Archer.Controlls.Systems.InfoPopup {
    public class InfoPopupControler : MonoBehaviour {
        [SerializeField] private string infoPopupSceneName;
        [SerializeField] private float popupTimeGap;

        private Queue<Popup> popupQueue;
        private Coroutine pendingRoutine;
        private IPopup popupControler;

        public bool IsEmpty => popupQueue.Count == 0;

        private void Start() {
            popupQueue = new Queue<Popup>();
        }
        private void Update() {
            
        }

        private bool CheckNecessity() {
            if (popupQueue.Count > 0 || popupControler != null && !popupControler.isViewed) {
                return true;
            } else {
                if (popupControler != null) {
                    UnloadPopupScene();
                }
                return false;
            }
        }

        private void Conect(IPopup popup) {
        }

        public void AddPopup(IEnumerable<Popup> popups) {
            foreach (var popup in popups) {
                popupQueue.Enqueue(popup);
            }
        }

        public void AddPopup(Popup popup) {
            popupQueue.Enqueue(popup);
        }

        private void LoadPopupScene() {
            StartCoroutine(LoadPopupSceneAsync());
        }
        private void UnloadPopupScene() {
            StartCoroutine(UnloadPopupSceneAsync());
        }


        private IEnumerator LoadPopupSceneAsync() {
            var load = SceneManager.LoadSceneAsync(infoPopupSceneName, LoadSceneMode.Additive);
            yield return load;
        }
        private IEnumerator UnloadPopupSceneAsync() {
            var unload = SceneManager.UnloadSceneAsync(infoPopupSceneName, UnloadSceneOptions.None);
            yield return unload;
        }
    }

    public interface IPopup {
        bool isViewed { get; }
    }

    public struct Popup {

    }

}
