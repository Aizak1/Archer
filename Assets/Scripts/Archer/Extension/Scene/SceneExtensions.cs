using UnityEngine;
using UnityEngine.SceneManagement;

namespace Archer.Extension.Scenes {
    public static class SceneExtensions {
        public static T GetComponent<T>(this Scene scene) where T : MonoBehaviour {
            var firstLevelGOArray = scene.GetRootGameObjects();
            foreach (var topLevelGO in firstLevelGOArray) {
                var component = topLevelGO.GetComponentInChildren<T>();
                if (component != null)
                    return component;
            }
            return null;
        }
    }
}
