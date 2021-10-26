using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archer.ArcherControlls { 
    [RequireComponent(typeof(ShootingController))]
    public class ShootingControllerSimulator : MonoBehaviour {
        [SerializeField] private bool isLoop;
        [SerializeField] private List<ShootingCycleSpec> shootingSimulationSpecList;

        private ShootingController shottingController;
        private Coroutine pendingRoutine;

        private void Start() {
            shottingController = GetComponent<ShootingController>();
        }

        private void Update() {
            if (isLoop && pendingRoutine == null) {
                pendingRoutine = StartCoroutine(SimulateShootin());
            }
        }

        private IEnumerator SimulateShootin() {
            for (int i = 0; i < shootingSimulationSpecList.Count; i++) {
                var spec = shootingSimulationSpecList[i];
                var preTimer = 0f;

                while(preTimer < spec.TimeGap) {
                    preTimer += Time.deltaTime;
                    yield return Time.deltaTime;
                }

                var timer = 0f;
                while(timer < spec.Time) {
                    shottingController.OuterControl(spec.Angle, spec.Force, false);
                    timer += Time.deltaTime;
                    yield return Time.deltaTime;
                }

                shottingController.OuterControl(spec.Angle, spec.Force, true);
            }
            pendingRoutine = null;
            yield return null;
        }
    }

    [Serializable]
    public struct ShootingCycleSpec {
        public ShootingCycleSpec(float force, float angle, float time, float timeGap) {
            this.force = force;
            this.angle = angle;
            this.time = time;
            this.timeGap = timeGap;
        }

        [Range(.5f, 3)] [SerializeField] private float timeGap;
        [Range(0,1)] [SerializeField]private float force;
        [Range(-90, 90)] [SerializeField] private float angle;
        [Range(.5f, 3)] [SerializeField] private float time;
        public float Force => force;
        public float Angle => angle;
        public float Time => time;
        public float TimeGap => timeGap;
    }
}
