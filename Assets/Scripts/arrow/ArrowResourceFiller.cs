using UnityEngine;
using UnityEngine.UI;

namespace arrow {
    public class ArrowResourceFiller : MonoBehaviour {
        [SerializeField]
        private GameObject[] arrowPrefabs;
        [SerializeField]
        private ArrowResource resource;

        private void Awake() {
            for (int i = 0; i < arrowPrefabs.Length; i++) {
                var arrowComponent = arrowPrefabs[i].GetComponentInChildren<Arrow>();

                if (arrowComponent == null) {
                    Debug.LogError("Invalid prefab");
                    continue;
                }

                if (resource.arrowPrefabs.ContainsKey(arrowComponent.arrowType)) {
                    Debug.LogError("Resources already has this type of arrow");
                    continue;
                }

                resource.arrowPrefabs.Add(arrowComponent.arrowType, arrowPrefabs[i]);
                resource.countToArrowType.Add(i, arrowComponent.arrowType);
                resource.arrowTypeToCount.Add(arrowComponent.arrowType, i);


            }
        }
    }
}

