using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Archer.Extension.Vector3Extension;
using Archer.DataStructure.Levels;
using Archer.Specs.LevelSpec;

namespace Archer.Controlls.UI.ShootingControlls {
    public class LevelMenu : MonoBehaviour {
        [Space]
        [SerializeField] private RectTransform containerTran;
        [Space]
        [SerializeField] private LevelGroup leftGroup;
        [SerializeField] private RectTransform leftAnchore;
        [Space]
        [SerializeField] private LevelGroup middleGroup;
        [SerializeField] private RectTransform middleAnchore;
        [Space]
        [SerializeField] private LevelGroup rightGroup;
        [SerializeField] private RectTransform rightAnchore;
        [Space]
        [SerializeField] private float snapSpeed;
        [SerializeField] private float width;

        private List<LevelDescriptor> levelsDescriptor;
        private List<LevelResult> levelsResult;
        private int middleStartIndex;

        Vector3? prevMousePos;

        private void Start() {
            middleStartIndex = 0;
            levelsDescriptor = new List<LevelDescriptor>();
            levelsResult = new List<LevelResult>();
            Show();
        }

        private void Update() {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (MobileControls())
                return;
#endif
#if UNITY_EDITOR
            if (EditorControls())
                return;
#endif
            UpdateSnap();
        }

        private void Init(List<LevelDescriptor> levelDescriptorsList, List<LevelResult> levelResults) {
            middleStartIndex = 0;
            levelsDescriptor = levelDescriptorsList;
            levelsResult = levelResults;
            Show();
        }

        private bool MobileControls() {
            var touchCount = Input.touchCount;
            if (touchCount == 1) {
                var touch = Input.GetTouch(0);
                var screenWidth = Screen.width;
                var deltaPos = touch.deltaPosition;
                var contaienrWidth = containerTran.rect.width;
                var pos = containerTran.anchoredPosition3D;
                var moverPercent = deltaPos.x / screenWidth;
                var newDeltaX = moverPercent * contaienrWidth / 2;
                var clamp = contaienrWidth * 0.5f;
                var clampLeft = clamp;
                var clampRight = clamp;
                if (!leftGroup.IsAnyActive)
                    clampRight = width / 2;
                if (!rightGroup.IsAnyActive)
                    clampLeft = width / 2;

                var newPos = Mathf.Clamp(
                    pos.x + newDeltaX, -clampLeft, clampRight);
                containerTran.anchoredPosition3D = new Vector3(newPos, pos.y, pos.z);
                return true;
            }
            return false;
        }

        private bool EditorControls() {
            var isPush = Input.GetMouseButton(2);
            if (isPush) {
                var mousePos = Input.mousePosition;
                if (prevMousePos != null) {
                    var prevPos = (Vector3)prevMousePos;
                    var screenWidth = Screen.width;
                    var deltaPos = mousePos - prevPos;
                    var contaienrWidth = containerTran.rect.width;
                    var pos = containerTran.anchoredPosition3D;
                    var moverPercent = deltaPos.x / screenWidth;
                    var newDeltaX = moverPercent * contaienrWidth / 2;
                    var clamp = contaienrWidth * 0.5f;
                    var clampLeft = clamp;
                    var clampRight = clamp;
                    if (!leftGroup.IsAnyActive)
                        clampRight = width / 2;
                    if (!rightGroup.IsAnyActive)
                        clampLeft = width / 2;

                    var newPos = Mathf.Clamp(
                        pos.x + newDeltaX, -clampLeft, clampRight);
                    containerTran.anchoredPosition3D = new Vector3(newPos, pos.y, pos.z);
                }
                prevMousePos = mousePos;
                return true;
            }
            prevMousePos = null;
            return false;

        }

        private void UpdateSnap() {
            var snapAnchor = GetClosestGroup();
            var ancorePos = snapAnchor.anchoredPosition3D;
            var containerPos = containerTran.anchoredPosition3D;
            if (ancorePos != containerPos) {
                var dir = new Vector3((ancorePos - containerPos).x, 0, 0).normalized;
                var newPos = containerPos + dir * snapSpeed * Time.deltaTime;

                if (ancorePos.OnLine(containerPos, newPos)) {
                    containerTran.anchoredPosition3D = ancorePos;
                    UpdateGroups();
                } else {
                    containerTran.anchoredPosition3D = newPos;
                }
            }
        }

        private void UpdateGroups() {
            var containerPos = containerTran.anchoredPosition3D;
            var leftAnchorePos = leftAnchore.anchoredPosition3D;
            var rightAnchorePos = rightAnchore.anchoredPosition3D;
            if (containerPos == leftAnchorePos) {
                middleStartIndex -= middleGroup.MocksCount;
                containerTran.anchoredPosition3D = middleAnchore.anchoredPosition3D;
                Show();
                return;
            }
            if (containerPos == rightAnchorePos) {
                middleStartIndex += middleGroup.MocksCount;
                containerTran.anchoredPosition3D = middleAnchore.anchoredPosition3D;
                Show();
                return;
            }
        }

        private RectTransform GetClosestGroup() {
            var containerOffset = containerTran.anchoredPosition3D;
            var leftPos = leftAnchore.anchoredPosition3D + containerOffset;
            var middlePos = middleAnchore.anchoredPosition3D + containerOffset;
            var rightPos = rightAnchore.anchoredPosition3D + containerOffset;

            var leftDist = Vector3.SqrMagnitude(leftPos);
            var middleDist = Vector3.SqrMagnitude(middlePos);
            var rightDist = Vector3.SqrMagnitude(rightPos);

            if (middleDist <= rightDist && middleDist <= leftDist) {
                return middleAnchore;
            }

            if (rightDist <= middleDist && rightDist <= leftDist) {
                return leftAnchore;
            }

            return rightAnchore;
        }

        private void Show() {
            SetupGroup(middleGroup, middleStartIndex);

            var startRightIndex = middleStartIndex + middleGroup.MocksCount;
            SetupGroup(rightGroup, startRightIndex);

            var startLeftIndex = middleStartIndex - leftGroup.MocksCount;
            SetupGroup(leftGroup, startLeftIndex);
        }

        private void SetupGroup(LevelGroup levelGroup, int startIndex) {
            if (startIndex > levelsDescriptor.Count || startIndex < 0) {
                //levelGroup.Setup(new List<LevelDescriptor>());
                return;
            }

            var rangleLenght = levelGroup.MocksCount;
            var rangeLenghtLimit = levelsDescriptor.Count - startIndex;
            if (rangleLenght > rangeLenghtLimit)
                rangleLenght = rangeLenghtLimit;

            var range = levelsDescriptor.GetRange(startIndex, rangleLenght);
            //levelGroup.Setup(range);
        }
    }
}
