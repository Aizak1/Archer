using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Archer.Controlls.IHitableAction;

namespace Archer.Controlls.Systems.DynamicOclusion {
    [RequireComponent(typeof(Collider))]
    public class DunamicOclusionObjectsControler : MonoBehaviour {
        [SerializeField] private GameObject prefab;
        [SerializeField] private int prefabPoolLength;
        [SerializeField] private int curruptPoolLength;
        [SerializeField] private float spawnGap;

        private List<ArrowObjectCorruptable> poolList;
        private List<ArrowObjectCorruptable> corruptPoolList;

        private float timeFromLastSpawn;

        private void Start() {
            corruptPoolList = new List<ArrowObjectCorruptable>();
            poolList = new List<ArrowObjectCorruptable>();
            for (int i = 0; i < prefabPoolLength; i++) {
                var corruptable = Instantiate(
                    prefab, transform).GetComponent<ArrowObjectCorruptable>();
                corruptable.gameObject.SetActive(false);
                corruptable.TrySetIsAvalible();
                poolList.Add(corruptable);
            }
            timeFromLastSpawn = 0;
        }

        private void Update() {
            CheckForPoolChange();
            timeFromLastSpawn += Time.deltaTime;
            if(timeFromLastSpawn > spawnGap) {
                if (TryGetFirstAvalible(out var curruotable)) {
                    curruotable.TryToLaunch();
                    timeFromLastSpawn = 0;
                }
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.gameObject.TryGetComponent(
                out ArrowObjectCorruptable corruptable) && !corruptable.IsStateChange) {
                if (!corruptable.IsCorrupted) {
                    corruptable.transform.position = transform.position;
                    corruptable.gameObject.SetActive(false);
                    corruptable.TrySetIsAvalible();
                } else {
                    var index = poolList.FindIndex(
                        0, poolList.Count, (item) => item.Equals(corruptable));
                    if (index != -1) {
                        poolList.RemoveAt(index);
                    }
                    Destroy(corruptable.gameObject);
                    var newCorruptable = Instantiate(
                        prefab, transform).GetComponent<ArrowObjectCorruptable>();
                    newCorruptable.gameObject.SetActive(false);
                    newCorruptable.TrySetIsAvalible();
                    poolList.Add(newCorruptable);
                }
            }
        }

        private void AddToCurruptPool(ArrowObjectCorruptable currupt) {
            if (curruptPoolLength > corruptPoolList.Count) {
                corruptPoolList.Add(currupt);
            } else {
                var firstCurrupted = corruptPoolList[0];
                corruptPoolList.RemoveAt(0);
                corruptPoolList.Add(currupt);
                Destroy(firstCurrupted.gameObject);
            }
        }

        private void CheckForPoolChange() {
            for (int i = 0; i < poolList.Count; i++) {
                var corruptable = poolList[i];
                if (corruptable.IsStateChange) {
                    var newCorruptable = Instantiate(
                        prefab, transform).GetComponent<ArrowObjectCorruptable>();
                    newCorruptable.gameObject.SetActive(false);
                    newCorruptable.TrySetIsAvalible();
                    poolList[i] = newCorruptable;
                    AddToCurruptPool(corruptable);
                }
            }
        }

        private bool TryGetFirstAvalible(out ArrowObjectCorruptable curruotable) {
            var index = -1;

            for (int i = 0; i < poolList.Count; i++) {
                var element = poolList[i];
                if (element.IsAvalible && !element.IsCorrupted) {
                    index = i;
                    break;
                }
            }

            if (index == -1) {
                curruotable = null;
                return false;
            }

            var firstAvalibleElement = poolList[index];
            curruotable = firstAvalibleElement;
            return true;
        }
    }
}
