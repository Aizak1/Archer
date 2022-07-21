using System.Collections;
using System.Collections.Generic;
using arrow;
using UnityEngine;

namespace arrow
{
    public class ArrowSpawnObject : MonoBehaviour
    {
        [SerializeField] private Arrow _currentArrow;

        public Arrow CurrentArrow => _currentArrow;
    }
}

