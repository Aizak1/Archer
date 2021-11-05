using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Archer.DataStructure.Persistance {
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
