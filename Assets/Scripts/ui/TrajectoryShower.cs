using bow;
using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ui {
    public class TrajectoryShower : MonoBehaviour {
        
        [SerializeField] private Bow _bow;
        [SerializeField] private TrajectorySettings[] arrowTrajectorySettings;
        [SerializeField]private LineRenderer lineRenderer;
        
        [SerializeField] private GameObject[] balls;
        [SerializeField] private float ballDistance = 0.4f;

        private TrajectorySettings _trajectorySetUp;
        private NativeArray<Vector3> _pointList;

        private int _ballIndex;
        private const string MAIN_TEXTURE = "_MainTex";

        private void OnEnable()
        {
            _bow.OnStartPull += StartDraw;
            _bow.OnPull += Draw;
            _bow.OnEndPull += EndDraw;
        }

        private void Start() {
            if (!lineRenderer) {
                Debug.LogError("No LineRenderer on trajectory shower");
                return;
            }
            lineRenderer.positionCount = 0;
            SetSettings(0);
        }
        private void StartDraw() {
            if (!lineRenderer) {
                return;
            }

            if (_bow.instantiatedArrow == null)
            {
                return;
            }

            lineRenderer.startWidth = _trajectorySetUp.StartWidth;
            lineRenderer.endWidth = _trajectorySetUp.EndWidth;
            lineRenderer.positionCount = _trajectorySetUp.numberOfPoitns;
        }

  
        private void Draw() {
            if (!lineRenderer) {
                return;
            }
            
            if (_bow.instantiatedArrow == null)
            {
                return;
            }

            var job = new CalculatePositionJob
            {
                pointsPositions = _pointList,
                pullAmount = _bow.pullAmount,
                speed = _bow.instantiatedArrow.Speed,
                arrowPosition = _bow.instantiatedArrow.transform.position,
                direction = _bow.instantiatedArrow.transform.forward,
                gravity = Physics.gravity,
                spaceBetweenPoints = _trajectorySetUp.spaceBetweenPoints
            };
            var handle = job.Schedule(_pointList.Length,0);
            handle.Complete();
            // for (int i = 0; i < _pointList.Length; i++) {
            //     var time = (i + 1) * _trajectorySetUp.spaceBetweenPoints;
            //     _pointList[i] = CalculatePointPosition(time);
            // }

            for (int i = 0; i < _trajectorySetUp.numberOfPoitns; i++) {
                var pos = _pointList[i];
                lineRenderer.SetPosition(i, pos);
            }

            if (balls.Length == 0) {
                return;
            }

            float[] magnitudes = new float[_pointList.Length - 1];
            float sum = 0;

            for (int i = 0; i < magnitudes.Length; i++) {
                magnitudes[i] = (_pointList[i] - _pointList[i + 1]).magnitude;
                sum += magnitudes[i];
            }

            float requiredMag = ballDistance;
            float tmpMag = 0;
            _ballIndex = 0;

            for (int i = 0; i < magnitudes.Length; i++) {
                if (tmpMag >= sum) {
                    break;
                }

                if (tmpMag + magnitudes[i] >= requiredMag) {
                    float need = requiredMag - tmpMag;
                    float full = magnitudes[i];

                    float percent = need / full;

                    Vector3 line = _pointList[i + 1] - _pointList[i];

                    balls[_ballIndex].transform.position = _pointList[i] + line * percent;
                    balls[_ballIndex].SetActive(true);
                    _ballIndex++;

                    tmpMag += magnitudes[i];
                    requiredMag += ballDistance;
                } else {
                    tmpMag += magnitudes[i];
                }
            }

            for (int i = _ballIndex; i < balls.Length; i++) {
                balls[i].SetActive(false);
            }

            if (_ballIndex == 2) {
                balls[0].transform.localScale = Vector3.one * _trajectorySetUp.StartWidth;
                balls[1].transform.localScale = Vector3.one * _trajectorySetUp.EndWidth;
                return;
            } else if (_ballIndex == 1) {
                balls[0].transform.localScale = Vector3.one * _trajectorySetUp.StartWidth;
                return;

            } else if (_ballIndex - 2 < 0) {
                return;
            }

            float deltaWidth = (_trajectorySetUp.StartWidth - _trajectorySetUp.EndWidth);
            float scaleStep = deltaWidth / (_ballIndex - 1);
            float scale = _trajectorySetUp.StartWidth;

            for (int i = 0; i < _ballIndex; i++) {
                balls[i].transform.localScale = Vector3.one * scale;
                scale -= scaleStep;
            }
        }

        private void EndDraw() {
            if (!lineRenderer) {
                return;
            }

            lineRenderer.positionCount = 0;

            if (balls.Length == 0) {
                return;
            }

            for (int i = 0; i <= _ballIndex; i++) {
                if (i >= balls.Length) {
                    return;
                }
                balls[i].SetActive(false);
            }
        }

        public void SetSettings(int presetIndex) {
            _trajectorySetUp = arrowTrajectorySettings[presetIndex];
            ApplySettings();
        }

        private void ApplySettings() {
            if (!lineRenderer) {
                return;
            }

            lineRenderer.colorGradient = _trajectorySetUp.Gradient;
            var XTille = _trajectorySetUp.XTille;
            lineRenderer.material.SetTextureScale(MAIN_TEXTURE, new Vector2(XTille, 1));
            _pointList = new NativeArray<Vector3>(_trajectorySetUp.numberOfPoitns,Allocator.Persistent);
        }

        private Vector3 CalculatePointPosition(float t) {
            var arrowTransform = _bow.instantiatedArrow.transform;
            var direction = arrowTransform.forward;
            var vO = _bow.instantiatedArrow.Speed * _bow.pullAmount * direction;
            return arrowTransform.position + (vO * t) + (t * t) * 0.5f * Physics.gravity;
        }
        

        private void OnDisable()
        {
            _bow.OnStartPull -= StartDraw;
            _bow.OnPull -= Draw;
            _bow.OnEndPull -= EndDraw;
            _pointList.Dispose();
        }
    }

    [BurstCompile]
    public struct CalculatePositionJob : IJobParallelFor
    {
        public NativeArray<Vector3> pointsPositions;
        [ReadOnly] public Vector3 arrowPosition;
        [ReadOnly]public Vector3 direction;
        [ReadOnly]public Vector3 gravity;
        [ReadOnly]public float speed;
        [ReadOnly]public float spaceBetweenPoints;
        [ReadOnly]public float pullAmount;
     


        public void Execute(int index)
        {
            var vO = speed * pullAmount * direction;
            var time = (index + 1) * spaceBetweenPoints;
            pointsPositions[index] = arrowPosition + (vO * time) + (time * time) * 0.5f * gravity;
        }
    }

    [Serializable]
    public struct TrajectorySettings {
        public Gradient Gradient;
        public float XTille;
        public float StartWidth;
        public float EndWidth;
        public float spaceBetweenPoints;
        public int numberOfPoitns;
    }
}