using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace arrow {
    public class ArrowResource : MonoBehaviour {
        public Dictionary<ArrowType, GameObject> arrowPrefabs
            = new Dictionary<ArrowType, GameObject>();
    }
}

