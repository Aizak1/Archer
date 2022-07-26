using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace level {
    public class SceneLoader : MonoBehaviour {

       
        [SerializeField] private Animator transitionAnimator;

       
        [SerializeField] private bool isFadeOut;

        private readonly int _fadeInTriggerId = Animator.StringToHash("FadeIn");
        private readonly int _fadeOutTriggerId = Animator.StringToHash("FadeOut");

        private readonly float TRANSITION_TIME = 2f / 3f;

        private void Awake() {
            if (isFadeOut) {
                transitionAnimator.SetTrigger(_fadeOutTriggerId);
            }
        }

        public void RestartLevel() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void LoadLevel(int index) {
            SceneManager.LoadScene(index);
        }

        public void LoadLevel(string name) {
            SceneManager.LoadScene(name);
        }

        public void LoadMenu() {
            SceneManager.LoadScene(0);
        }

        private IEnumerator FadeLoadLevel(string name) {
            transitionAnimator.SetTrigger(_fadeInTriggerId);
            yield return new WaitForSeconds(TRANSITION_TIME);
            LoadLevel(name);
        }

        private IEnumerator FadeLoadLevel(int index) {
            transitionAnimator.SetTrigger(_fadeInTriggerId);
            yield return new WaitForSeconds(TRANSITION_TIME);
            LoadLevel(index);
        }

        public void LoadNextLevel() {
            var nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextLevelIndex == SceneManager.sceneCountInBuildSettings) {
                nextLevelIndex = 0;
            }
            StartCoroutine(FadeLoadLevel(nextLevelIndex));
        }

        public void FadeLoadMenu() {
            StartCoroutine(FadeLoadLevel(0));
        }

        public void FadeLoadLevelByName(string name) {
            StartCoroutine(FadeLoadLevel(name));
        }

        public void FadeLoadLevelByIndex(int index) {
            StartCoroutine(FadeLoadLevel(index));
        }

        public void Quit() {
            Application.Quit();
        }

        public void Reset() {
            PlayerPrefs.DeleteAll();
        }
    }
}