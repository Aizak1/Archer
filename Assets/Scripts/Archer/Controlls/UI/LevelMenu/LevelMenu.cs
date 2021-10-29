using System;
using System.Collections.Generic;
using UnityEngine;

namespace Archer.Controlls.UI.ShootingControlls {
    public class LevelMenu : MonoBehaviour {
        [Header("Temp")]
        [SerializeField] private List<LevelStatDescriptor> levelStatDescriptors;
        [Space]
        [SerializeField] private RectTransform containerTran;
        [Space]
        [SerializeField] private LevelGroup leftGroup;
        [SerializeField] private RectTransform leftRectTran;
        [Space]
        [SerializeField] private LevelGroup middleGroup;
        [SerializeField] private RectTransform middleRectTran;
        [Space]
        [SerializeField] private LevelGroup rightGroup;
        [SerializeField] private RectTransform rightRectTran;
        [Space]
        [SerializeField] private float snapSpeed;

        private List<LevelStatDescriptor> levelsDescriptor;
        private int middleStartIndex;

        private void Start() {
            middleStartIndex = 0;
        }

        Vector3? prevMousePos;
        private void Update() {
            var touchCount = Input.touchCount;
            if (touchCount == 1) {
                var touch = Input.GetTouch(0);
                var screenWidth = Screen.width;
                var deltaPos = touch.deltaPosition;
                var contaienrWidth = containerTran.rect.width;
                var pos = containerTran.position;
                var moverPercent = deltaPos.x / screenWidth;
                var newDeltaX = moverPercent * contaienrWidth / 2;
                containerTran.position = new Vector3(pos.x + newDeltaX, pos.y, pos.z);
                return;
            }


            var isPush = Input.GetMouseButton(2);
            if (isPush) {

                var mousePos = Input.mousePosition;
                if (prevMousePos != null) {
                    var prevPos = (Vector3)prevMousePos;
                    var screenWidth = Screen.width;
                    var deltaPos = mousePos - prevPos;
                    var contaienrWidth = containerTran.rect.width;
                    var pos = containerTran.position;
                    var moverPercent = deltaPos.x / screenWidth;
                    var newDeltaX = moverPercent * contaienrWidth / 2;
                    containerTran.position = new Vector3(pos.x + newDeltaX, pos.y, pos.z);
                }
                prevMousePos = mousePos;
                return;
            }
            prevMousePos = null;

            if (containerTran.localPosition != Vector3.zero) {
                var anchor = GetClosestGroup();
                var localOffset = anchor.localPosition;
                //containerTran.localPosition = -localOffset;
                var localPos = containerTran.position;
                var dir = (localOffset).normalized;
                var newPos = localPos + dir * snapSpeed * Time.deltaTime;
                Debug.Log(localOffset);
                Debug.Log(localPos);
                Debug.Log(dir);
                Debug.Log(newPos);
                containerTran.position = newPos;

            }
        }

        private RectTransform GetClosestGroup() {
            var containerOffset = containerTran.localPosition;
            var leftPos = leftRectTran.localPosition + containerOffset;
            var middlePos = middleRectTran.localPosition + containerOffset;
            var rightPos = rightRectTran.localPosition + containerOffset;

            var leftDist = Vector3.SqrMagnitude(leftPos);
            var middleDist = Vector3.SqrMagnitude(middlePos);
            var rightDist = Vector3.SqrMagnitude(rightPos);

            if (middleDist < rightDist && middleDist < leftDist) {
                return middleRectTran;
            }

            if (rightDist <= middleDist && rightDist <= leftDist) {
                return rightRectTran;
            }

            return leftRectTran;
        }

        private void Show(List<LevelStatDescriptor> levelsDescriptor) {
            this.levelsDescriptor = levelsDescriptor;
            SetupGroup(middleGroup, middleStartIndex);

            var startRightIndex = middleStartIndex + middleGroup.MocksCount + 1;
            SetupGroup(rightGroup, startRightIndex);
        }

        private void SetupGroup(LevelGroup levelGroup, int startIndex) {
            if (startIndex > levelsDescriptor.Count)
                return;

            var rangleLenght = levelGroup.MocksCount;
            var rangeLenghtLimit = levelsDescriptor.Count - startIndex;
            if (rangleLenght > rangeLenghtLimit)
                rangleLenght = rangeLenghtLimit;

            var range = levelsDescriptor.GetRange(startIndex, rangleLenght);
            Debug.Log(range.Count);
            levelGroup.Setup(range);

        }

        [ContextMenu("Show")]
        private void DebugShow() {
            Show(levelStatDescriptors);
        }
    }

    [Serializable]
    public struct LevelStatDescriptor {
        public LevelStatDescriptor(int id, List<int> scoreLowerBounds, int score = 0) {
            this.id = id;
            this.scoreLowerBounds = scoreLowerBounds;
            this.score = score;
        }

        [SerializeField] private int id;
        public int Id => id;

        [SerializeField] private int score;
        public int Score => score;

        [SerializeField] private List<int> scoreLowerBounds;
        public IEnumerable<int> ScoreLowerBounds => scoreLowerBounds;
    }
}
