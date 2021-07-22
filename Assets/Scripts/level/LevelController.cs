using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hittable;

public class LevelController : MonoBehaviour
{
    [SerializeField]
    private SceneLoader sceneLoader;

    private int enemiesCount;

    private void Awake() {
        enemiesCount = FindObjectsOfType<Enemy>().Length;
    }

    private void Update() {
        if(enemiesCount == 0) {
            sceneLoader.RestartLevel();
        }
    }

    public void DecreaseEnemyCount() {
        enemiesCount--;
    }
}
