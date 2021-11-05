using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archer.Controlls.Systems.Persistance {
    //TODO async save load
    //TODO versionSupport
    public class DataOperator : MonoBehaviour {
        [SerializeField] private SceneControler sceneControler;
        [SerializeField] private GameData gameData;
        [SerializeField] private GameData loadGameData;

        private string saveGameName = "gamesave.save";
        private string saveGamePath;
        private Coroutine pendingRoutine;

        private void Start() {
            var pathArray = new string[] {
                Application.persistentDataPath,
                saveGameName,
            };
            saveGamePath = Path.Combine(pathArray);
        }

        [ContextMenu("load")]
        public void LoadGame() {
            if (!File.Exists(saveGamePath)) {
                Debug.Log("save data not found");
                return;
            }
            var json = File.ReadAllText(saveGamePath);
            loadGameData = JsonUtility.FromJson<GameData>(json);
        }

        private void StartMenu() {
            sceneControler.TryLoadStartScene();
        }

        [ContextMenu("save")]
        private void SaveGame() {
            var json = JsonUtility.ToJson(gameData);
            File.WriteAllText(saveGamePath, json);
        }
    }


    [Serializable]
    public struct GameData {
        public GameData(Player player, GameSettings gameSettings) {
            this.player = player;
            this.gameSettings = gameSettings;
        }

        [SerializeField] private GameSettings gameSettings;
        public GameSettings GameSettings => gameSettings;

        [SerializeField] private Player player;
        public Player Player => player;

    }


    [Serializable]
    public struct GameSettings {
        public GameSettings(int version) {
            this.version = version;
        }

        [SerializeField] private int version;
        public int Version => version;
    }

    [Serializable]
    public struct Player {
        public Player(int id) {
            this.id = id;
        }

        [SerializeField] private int id;
        public int Id => id;
    }
}
