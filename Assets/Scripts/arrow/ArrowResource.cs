using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace arrow {
    public class ArrowResource : MonoBehaviour {
        public Dictionary<ArrowType, GameObject> arrowPrefabs
            = new Dictionary<ArrowType, GameObject>();

        public Dictionary<int, ArrowType> countToArrowType = new Dictionary<int, ArrowType>();

        public Dictionary<ArrowType, int> arrowTypeToCount = new Dictionary<ArrowType, int>();
    }
}

