using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace level {
    public class SceneLoader : MonoBehaviour {

        [SerializeField]
        private Animator transitionAnimator;

        private int transitionTriggerId = Animator.StringToHash("StartTransition");

        private float TRANSITION_TIME = 2f / 3f;

        public void RestartLevel() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private IEnumerator LoadLevel(string name) {
            transitionAnimator.SetTrigger(transitionTriggerId);
            yield return new WaitForSeconds(TRANSITION_TIME);
            SceneManager.LoadScene(name);
        }

        private IEnumerator LoadLevel(int index) {
            transitionAnimator.SetTrigger(transitionTriggerId);
            yield return new WaitForSeconds(TRANSITION_TIME);
            SceneManager.LoadScene(index);
        }

        public void LoadNextLevel() {
            var nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if(nextLevelIndex == SceneManager.sceneCountInBuildSettings) {
                nextLevelIndex = 0;
            }
            StartCoroutine(LoadLevel(nextLevelIndex));
        }

        public void LoadMenu() {
            StartCoroutine(LoadLevel(0));
        }

        public void LoadLevelByName(string name) {
            StartCoroutine(LoadLevel(name));
        }

        public void LoadLevelByIndex(int index) {
            StartCoroutine(LoadLevel(index));
        }

        public void Quit() {
            Application.Quit();
        }

        public void Reset() {
            PlayerPrefs.DeleteAll();
        }
    }
}