using System.Collections.Generic;
using UnityEngine;

namespace arrow {
    public class ArrowResource : MonoBehaviour
    {
        [SerializeField] private List<ArrowSpawnObject> _arrowsPrefabs;

        public List<ArrowSpawnObject> ArrowPrefabs => _arrowsPrefabs;
    }
}