using UnityEngine;
using DG.Tweening;
using level;
using ui.screen;
using UnityEngine.SceneManagement;

namespace ui {
    public class HintScreen : UiScreen{
        
        [SerializeField] private GameObject hintObject;
        [SerializeField] private Transform finalPosition;
        
        [SerializeField] private float finalScale;
        
        [SerializeField] private TrailRenderer trail;
        
        [SerializeField] private float time;
        

        private Vector3 _startPosition;

        private const int LEVEL_1 = 1;
        private const int LEVEL_18 = 18;

        private const float ACCURACY = 0.1f;

        private void OnEnable() {
            _startPosition = hintObject.transform.position;
            if (trail) 
            {
                trail.enabled = true;
            }

            int levelIndex = SceneManager.GetActiveScene().buildIndex;
            var pos = finalPosition.position;

            switch (levelIndex) 
            {
                case LEVEL_1:
                    var tween = hintObject.transform.DOMove(pos, time).SetLoops(100);
                    tween.OnStepComplete(ClearTrailAfterStep);
                    break;
                case LEVEL_18:
                    hintObject.transform.DOMove(pos, time).SetLoops(100, LoopType.Yoyo);
                    break;
            }
        }

        private void Update() {

            if ((hintObject.transform.position - _startPosition).magnitude < ACCURACY) 
            {
                if (trail) 
                {
                    trail.enabled = true;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                ClearTrailAfterStep();
                _levelController.ChangeGameState(GameState.InGame);
                enabled = false;
                hintObject.transform.DOKill();
            }
        }

        private void ClearTrailAfterStep() 
        {
            if (trail) 
            {
                trail.Clear();
                trail.enabled = false;
            }
        }
    }
}