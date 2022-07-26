using UnityEngine;
using hittable;
using bow;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace level {
    [DisallowMultipleComponent]
    public class LevelController : MonoBehaviour
    {
        private GameState _currentGameState;
        
        [SerializeField] private Bow _bow;

        [SerializeField]private float _starTime;
        [SerializeField]private int _countOfArrowsForStar;

        [SerializeField] private bool _isStartFromManual;
        
        private int _targetsCount;
        private int _starConditionsCompleteCount;
        private float _timeSinceStart;
        
        public float StarTime => _starTime;
        public int CountOfArrowsForStar => _countOfArrowsForStar;
        public int StarConditionsCompleteCount => _starConditionsCompleteCount;
        public int TargetsCount => _targetsCount;
        public float TimeSinceStart => _timeSinceStart;
        public int ShotsCount => _bow.shotsCount;
        public Bow Bow => _bow;
        

        public UnityEvent OnTargetsDecrease;
        public UnityEvent<GameState> OnGameStateChanged;

        private void Awake() {

            var targets = FindObjectsOfType<Target>();
            _targetsCount = targets.Length;
            foreach (var item in targets)
            {
                item.OnTargetHitted.AddListener(DecreaseTargetsCount);
            }
            
            if(_countOfArrowsForStar == 0) {
                _countOfArrowsForStar = _targetsCount + 1;
            }
            _starConditionsCompleteCount = 1;
            OnGameStateChanged.AddListener(ControlBow);

            ChangeGameState(_isStartFromManual ? GameState.Manual : GameState.InGame);
        }

        private void Update() 
        {
            if (_currentGameState == GameState.InGame)
            { 
                _timeSinceStart += Time.deltaTime;
            }
        }

        public void DecreaseTargetsCount() {
            _targetsCount--;
            OnTargetsDecrease?.Invoke();
            if (_targetsCount == 0)
            {
                OnWin();
            }
        }

        public void ChangeGameState(GameState gameState)
        {
            _currentGameState = gameState;
            OnGameStateChanged?.Invoke(gameState);
        }

        private void OnWin()
        {
            if (_bow.shotsCount <= _countOfArrowsForStar) 
            {
                _starConditionsCompleteCount++;
            }

            if (_timeSinceStart <= _starTime) 
            {
                _starConditionsCompleteCount++;
            }
            
            var nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextLevelIndex == SceneManager.sceneCountInBuildSettings) 
            {
                nextLevelIndex = 0;
            }
            if (nextLevelIndex > PlayerPrefs.GetInt(LevelsManager.LEVEL_AT)) 
            {
                PlayerPrefs.SetInt(LevelsManager.LEVEL_AT, nextLevelIndex);
            }

            var starsAtLevels = PlayerPrefs.GetString(LevelsManager.STARTS_AT_LEVELS);

            if (nextLevelIndex - 1 > starsAtLevels.Length) 
            {
                var value = starsAtLevels + $"{_starConditionsCompleteCount}";
                PlayerPrefs.SetString(LevelsManager.STARTS_AT_LEVELS,value);
            } 
            else 
            {
                char starsAtLevel = starsAtLevels[nextLevelIndex - 2];
                if (_starConditionsCompleteCount > int.Parse(starsAtLevel.ToString())) 
                {
                    int index = nextLevelIndex - 2;
                    var value = _starConditionsCompleteCount.ToString();
                    var newValue = starsAtLevels.Remove(index,1).Insert(index,value);
                    PlayerPrefs.SetString(LevelsManager.STARTS_AT_LEVELS, newValue);
                }
            }
            enabled = false;
            ChangeGameState(GameState.Win);
        }

        public void OnFail()
        {
            enabled = false;
            ChangeGameState(GameState.Fail);
        }

        public void Pause()
        {
            ChangeGameState(GameState.Pause);
        }

        public void Resume()
        {
            ChangeGameState(GameState.InGame);
        }

        private void ControlBow(GameState gameState)
        {
            _bow.enabled = gameState == GameState.InGame || gameState == GameState.Hint;
        }
    }
}