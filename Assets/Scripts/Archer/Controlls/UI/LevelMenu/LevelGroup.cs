using System.Collections.Generic;
using Archer.DataStructure.Levels;
using UnityEngine;

namespace Archer.Controlls.UI.ShootingControlls {
    public class LevelGroup : MonoBehaviour {
        [SerializeField] private List<Level> levels;

        private int activeCount;

        public int MocksCount => levels.Count;
        public bool IsAnyActive => activeCount > 0;

        public void Setup(IEnumerable<LevelDescriptor> levelDescriptorList,
            IEnumerable<LevelResult> levelResultList) {
            activeCount = 0;
            var enumerator = levelDescriptorList.GetEnumerator();
            foreach (var level in levels) {
                if (enumerator.MoveNext()) {
                    level.gameObject.SetActive(true);
                    //level.Setup(enumerator.Current);
                    activeCount++;
                } else {
                    level.gameObject.SetActive(false);
                }
            }
        }
    }
}