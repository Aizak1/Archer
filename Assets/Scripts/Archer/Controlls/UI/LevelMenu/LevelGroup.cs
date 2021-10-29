using System.Collections.Generic;
using UnityEngine;

namespace Archer.Controlls.UI.ShootingControlls {
    public class LevelGroup : MonoBehaviour {
        [SerializeField] private List<Level> levels;

        public int MocksCount => levels.Count;

        public void Setup(IEnumerable<LevelStatDescriptor> levelDescriptorList) {
            var enumerator = levelDescriptorList.GetEnumerator();
            foreach (var level in levels) {
                if (enumerator.MoveNext())
                    level.Setup(enumerator.Current);
                else
                    level.gameObject.SetActive(false);
            }
        }
    }
}