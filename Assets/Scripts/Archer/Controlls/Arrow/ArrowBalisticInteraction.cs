using System;
using System.Collections.Generic;
using UnityEngine;
using Archer.Specs.Hitable;

namespace Archer.Controlls.ArrowControlls {
    [RequireComponent(typeof(Collider))]
    public class ArrowBalisticInteraction : MonoBehaviour {
        [SerializeField] private HitableSpec hitableSpec;
        [SerializeField] private Rigidbody rigid;
        [SerializeField] private GameObject sphere;
        [SerializeField] private bool isApplyForce;

        private List<ArrowImpulceDesc> arrowEnergyDescs;
        private int hitableLayerIndex;
        private bool isApplyForceToObject => isApplyForce && rigid != null;

        private void Start() {
            hitableLayerIndex = LayerMask.NameToLayer("ArrowHittable");
            arrowEnergyDescs = new List<ArrowImpulceDesc>();
        }

        private void FixedUpdate() {
            for (int i = 0; i < arrowEnergyDescs.Count; i++) {
                if (arrowEnergyDescs[i].Arrow == null) {
                    arrowEnergyDescs.RemoveAt(i);
                } else {
                    var localIndex = i;
                    UpdateArrowPos(localIndex, arrowEnergyDescs[i]);
                }
            }
        }

        public void PerformBalisticHit(ArrowController arrow) {
            var speed = arrow.ArrowRigid.velocity.magnitude;
            var arrowRigitSpec = arrow.RigidSpec;
            var arrowImpulce = arrowRigitSpec.Mass * speed;
            var arrowTran = arrow.transform;
            var arrowPos = arrowTran.position;
            var arrowDir = arrowTran.forward;
            var arrowLength = Vector3.Distance(arrow.transform.position, arrow.TipTran.position);

            var layerMask = (1 << hitableLayerIndex);
            var startHits = Physics.RaycastAll(arrowPos - arrowDir * 5, arrowDir, 10,
               layerMask, QueryTriggerInteraction.Collide);
            var startRes = FindHitObject(startHits, gameObject);
            if (startRes == null)
                return;

            var startHit = (RaycastHit)startRes;

            var dotProduct = Mathf.Abs(Vector3.Dot(arrowDir, startHit.normal));

            if (dotProduct < 0.2f) {
                var arrowRigid = arrow.ArrowRigid;
                var newArrowDir = Vector3.Reflect(arrowDir, startHit.normal);
                var hitArrowImpulce = arrowImpulce * dotProduct;
                var hitObjectImpulce = arrowImpulce * (1 - dotProduct);
                arrow.transform.position = startHit.point;
                arrowRigid.velocity = Vector3.zero;
                arrowRigid.AddForce(newArrowDir * hitArrowImpulce, ForceMode.Impulse);
                arrowRigid.rotation = Quaternion.LookRotation(
                    newArrowDir * hitArrowImpulce, transform.up);
                var objectForceDirection = (arrowDir - newArrowDir).normalized;
                if (isApplyForceToObject)
                    rigid.AddForceAtPosition(objectForceDirection * hitObjectImpulce,
                        startHit.point, ForceMode.Impulse);
                return;
            }

            /*
            if (arrowImpulce < hitableSpec.PenetrationThreshold) {
                var arrowRigid = arrow.ArrowRigid;
                var hitArrowImpulce = arrowImpulce * (1 - dotProduct);
                var hitObjectImpulce = arrowImpulce * dotProduct;

                var newArrowDir = -arrowDir;
                arrowRigid.velocity = Vector3.zero;
                arrowRigid.AddForce(newArrowDir * hitArrowImpulce, ForceMode.Impulse);

                if (isApplyForceToObject)
                    rigid.AddForceAtPosition(hitObjectImpulce * arrowDir,
                        startHit.point, ForceMode.Impulse);
                return;
            }
            */

            arrow.ToggleIsInObject(true);
            var newPos = startHit.point - arrowDir * arrowLength;
            arrow.transform.position = newPos;

            arrow.transform.SetParent(transform);
            var arrowDesk = new ArrowImpulceDesc(arrow, arrowImpulce);
            arrowEnergyDescs.Add(arrowDesk);
        }

        private void UpdateArrowPos(int index, ArrowImpulceDesc arrowEnergyDesc) {
            var arrow = arrowEnergyDesc.Arrow;
            var arrowImpulce = arrowEnergyDesc.ArrowImpulce;
            var arrowRigidSpec = arrow.RigidSpec;
            var arrowTran = arrow.transform;
            var arrowPos = arrowTran.position;
            var arrowDir = arrowTran.forward;

            var arrowLength = Vector3.Distance(arrow.transform.position, arrow.TipTran.position);
            var layerMask = (1 << hitableLayerIndex);
            var startHits = Physics.RaycastAll(arrowPos - arrowDir * 5, arrowDir, 10,
                layerMask, QueryTriggerInteraction.Collide);
            var endHits = Physics.RaycastAll(arrowPos + arrowDir * 5, -arrowDir, 10,
                layerMask, QueryTriggerInteraction.Collide);
            var startRes = FindHitObject(startHits, gameObject);
            var endRes = FindHitObject(endHits, gameObject);

            if (startRes == null || endRes == null)
                return;

            var startHit = (RaycastHit)startRes;
            var endHit = (RaycastHit)endRes;

            var speed = arrowImpulce / arrowRigidSpec.Mass;

            var passDistancePerIteration = speed * Time.deltaTime;
            var newPos = arrowTran.position + arrowDir * passDistancePerIteration;
            var impulceDiff = hitableSpec.AbsorbationPerUnit * passDistancePerIteration;
            var startVayPoint = startHit.point - arrowDir * arrowLength;
            var fullDistance = Vector3.Distance(startVayPoint, endHit.point);
            var passedDistance = Vector3.Distance(startVayPoint, newPos); 
            var passFactor = passedDistance / fullDistance;

            if (passFactor >= 1) {
                newPos = endHit.point;
                passedDistance = Vector3.Distance(arrowTran.position, newPos);
                var localImpulceDiff = hitableSpec.AbsorbationPerUnit * passedDistance;
                if (arrowImpulce - localImpulceDiff > 0) {
                    arrow.ToggleIsInObject(false);
                    var arrowRigid = arrow.ArrowRigid;
                    var force = arrowDir * (arrowImpulce - localImpulceDiff);
                    arrowRigid.AddForce(force, ForceMode.Impulse);
                    if (isApplyForceToObject)
                        rigid.AddForceAtPosition(force, arrowPos, ForceMode.Impulse);
                } else {
                    passedDistance = arrowImpulce / hitableSpec.AbsorbationPerUnit;
                    newPos = arrowPos + arrowDir * passedDistance;
                    if (isApplyForceToObject)
                        rigid.AddForceAtPosition(
                            arrowImpulce * arrowDir, arrowPos, ForceMode.Impulse);
                }
                arrow.transform.position = newPos;
                arrowEnergyDescs.RemoveAt(index);
                if (arrowImpulce - localImpulceDiff <= 0)
                    arrow.RemoveControl();
                return;
            }

            var newImpulce = arrowImpulce - impulceDiff;
            if (newImpulce <= 0.01f) {
                var newSpeed = arrowImpulce / arrowRigidSpec.Mass;
                passDistancePerIteration = newSpeed * Time.deltaTime;
                passDistancePerIteration = passDistancePerIteration * arrowImpulce / impulceDiff;
                newPos = arrowTran.position + arrowDir * passDistancePerIteration;
                var force = arrowDir * arrowImpulce;
                if (isApplyForceToObject)
                    rigid.AddForceAtPosition(force, arrowPos, ForceMode.Impulse);
                arrow.transform.position = newPos;
                arrowEnergyDescs.RemoveAt(index);
                arrow.RemoveControl();
                return;
            }

            if (isApplyForceToObject)
                rigid.AddForceAtPosition(newImpulce * arrowDir, arrowTran.position, ForceMode.Impulse);
            arrowTran.position = newPos;
            arrowEnergyDescs[index] = new ArrowImpulceDesc(arrow, newImpulce);
        }

        private RaycastHit? FindHitObject(RaycastHit[] hits, GameObject gm) {
            RaycastHit? resultHit = null;
            foreach (var hit in hits) {
                if (hit.collider.gameObject.Equals(gm)) {
                    resultHit = hit;
                    break;
                }
            }
            return resultHit;
        }

        [ContextMenu("Push")]
        private void Push() {
            rigid.AddForce(transform.forward * 10, ForceMode.Impulse);
        }
    }

    public struct ArrowImpulceDesc {
        public ArrowImpulceDesc(ArrowController arrow, float impulce) {
            this.arrow = arrow;
            this.arrowImpulce = impulce;
        }

        private ArrowController arrow;
        public ArrowController Arrow => arrow;

        private float arrowImpulce;
        public float ArrowImpulce => arrowImpulce;
    }
}
