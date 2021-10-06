using System;
using System.Collections;
using System.Text;
using Archer.Specs.Arrow;
using UnityEngine;
using Archer.Types.ArrowTypes;
using Archer.Controlls.ArrowHitableControlls;

namespace Archer.Controlls.ArrowControlls {
    public class ArrowController : MonoBehaviour
    {
        [SerializeField] private ArrowSpec arrowSpec;
        [SerializeField] private Rigidbody rigid;
        [SerializeField] private BoxCollider colider;
        [SerializeField] private TrailRenderer trail;
        [SerializeField] private Transform tip;

        public bool IsInAir => isInAir;
        public bool WasShoot => wasShoot;

        private Coroutine pendingRoutine;

        private Vector3 prevTipPos;
        private int hitableLayerIndex;
        private bool wasShoot;
        private bool isInAir;

        private void Start() {
            hitableLayerIndex = LayerMask.NameToLayer("ArrowHittable");
            Application.targetFrameRate = 30;
            prevTipPos = tip.position;
        }

        private void FixedUpdate() {

            if (!isInAir)
                return;

            if (isInAir && rigid.velocity != Vector3.zero)
                rigid.rotation = Quaternion.LookRotation(rigid.velocity, transform.up);
            var dir = (tip.position - prevTipPos).normalized;
            var layerMask = (1 << hitableLayerIndex);
            var tipDistance = (prevTipPos - tip.position).magnitude;
            if (tipDistance < 1 && Physics.Linecast(
               prevTipPos, tip.position, out RaycastHit hit, layerMask)) {
                var hitable = hit.collider.GetComponent<ArrowHitable>();
                var hitPoint = hit.point;
                var hitDir = hit.normal;
                var dot = Vector3.Dot(dir, hitDir);

                if (Mathf.Abs(dot) < 0.15f) {
                    Recoshet(dir, hitDir, hitable.Bounce);
                } else {
                    Hit(hitPoint, tip.position, hitable);
                }
            }

            prevTipPos = tip.position;
        }

        public void Release(float force) {
            isInAir = true;
            wasShoot = true;
            var direction = transform.forward;
            enabled = true;
            trail.enabled = true;
            rigid.useGravity = true;
            rigid.isKinematic = false;
            rigid.velocity = direction * force;
        }

        public void Release(Vector3 velocity) {
            wasShoot = true;
            isInAir = true;
            enabled = true;
            trail.enabled = true;
            rigid.useGravity = true;
            rigid.isKinematic = false;
            rigid.velocity = velocity;
        }

        public void Split() {
            if (isInAir && arrowSpec.IsSplitable) {
                Split(arrowSpec.SplitAngle, arrowSpec.SplitCount);
            }
        }

        private void Split(float angleBetweenSplitArrows, int splitArrowsAmount) {
            float angle = -angleBetweenSplitArrows * (splitArrowsAmount - 1) / 2;

            for (int i = 0; i < splitArrowsAmount; i++) {
                var instantiatedArrow = Instantiate(this, transform.position, transform.rotation);
                Vector3 velocity = rigid.velocity;

                float radAngle = angle * Mathf.Deg2Rad;

                float newY = Mathf.Sin(radAngle) * velocity.z + Mathf.Cos(radAngle) * velocity.y;
                float newZ = Mathf.Cos(radAngle) * velocity.z - Mathf.Sin(radAngle) * velocity.y;

                Vector3 newVelocity = new Vector3(velocity.x, newY, newZ);

                instantiatedArrow.Release(newVelocity);

                angle += angleBetweenSplitArrows;
            }

            Destroy(gameObject);
        }

        private IEnumerator GoThrowObject(Vector3 startPos, Vector3 endPos, ArrowHitable hitable) {
            var arrowLengthPosDiff = (tip.position - transform.position);
            var arrowStartPos = startPos - arrowLengthPosDiff;
            var inObjectPassDist = endPos - arrowStartPos;
            var inObjectDist = Vector3.Distance(arrowStartPos, endPos);

            var speed = rigid.velocity.magnitude;
            var dir = rigid.velocity.normalized;

            rigid.useGravity = false;
            rigid.isKinematic = false;
            rigid.velocity = Vector3.zero;
            arrowSpec.IsSplitable = false;

            var Ek = (arrowSpec.ArrowMass * speed * speed) / 2f;
            var hardnes = hitable.Hardnes;
            var isInBody = true;

            // - Ek/u;

            while (isInBody) {
                var newSpeed = (float)Math.Sqrt(Ek * 2f / arrowSpec.ArrowMass);
                var newPos = transform.position + dir * newSpeed * Time.deltaTime;
                var passDist = Vector3.Distance(transform.position, newPos);
                var Ekdiff = hardnes * passDist * arrowSpec.PenetrationFactor; // H * d * k
                var currEk = Ek - Ekdiff;
                if (currEk < 0) {
                    var passForFrameInObj = Ek / hardnes;
                    var recalcPos = transform.position 
                        + dir * passForFrameInObj * arrowSpec.PenetrationFactor;
                    var flowPassFactorInObj = Vector3.Distance(
                        arrowStartPos, recalcPos) / inObjectDist;
                    if (flowPassFactorInObj <= 1) {
                        transform.position = recalcPos;
                    } else {
                        var EkPassValue = Vector3.Distance(transform.position, endPos)
                            * hardnes * arrowSpec.PenetrationFactor;
                        var EkRest = Ek - EkPassValue;
                        var exitSpeed = (float)Math.Sqrt(EkRest * 2f / arrowSpec.ArrowMass);
                        enabled = true;
                        trail.enabled = true;
                        rigid.useGravity = true;
                        rigid.isKinematic = false;
                        rigid.velocity = dir * exitSpeed;
                        isInBody = false;

                    }
                    break;
                } else {
                    var flowPassFactorInObj = Vector3.Distance(
                        arrowStartPos, newPos) / inObjectDist;
                    if (flowPassFactorInObj < 1) {
                        transform.position = newPos;
                        Ek -= Ekdiff;
                        yield return Time.deltaTime;
                        continue;
                    } else {
                        var passDistToExit = Vector3.Distance(transform.position, endPos);
                        var EkPassToExit = passDistToExit * hardnes * arrowSpec.PenetrationFactor;
                        var EkRest = Ek - EkPassToExit;
                        var exitSpeed = (float)Math.Sqrt(EkRest * 2f / arrowSpec.ArrowMass);
                        enabled = true;
                        trail.enabled = true;
                        rigid.useGravity = true;
                        rigid.isKinematic = false;
                        rigid.velocity = dir * exitSpeed;
                        isInBody = false;
                        break;
                    }
                }
            }
            yield return Time.deltaTime;
        }

        private IEnumerator GoTrowEndlessObject(Vector3 startPos, Vector3 endPos, ArrowHitable hitable) {
            var arrowLengthPosDiff = (tip.position - transform.position);
            var arrowStartPos = startPos - arrowLengthPosDiff;
            var inObjectPassDist = endPos - arrowStartPos;
            var inObjectDist = Vector3.Distance(arrowStartPos, endPos);

            var speed = rigid.velocity.magnitude;
            var dir = rigid.velocity.normalized;

            rigid.useGravity = false;
            rigid.isKinematic = false;
            rigid.velocity = Vector3.zero;
            arrowSpec.IsSplitable = false;

            var Ek = (arrowSpec.ArrowMass * speed * speed) / 2f;
            var hardnes = hitable.Hardnes;

            while (Ek > 0) {
                var newSpeed = (float)Math.Sqrt(Ek * 2f / arrowSpec.ArrowMass);
                var newPos = transform.position + dir * newSpeed * Time.deltaTime;
                var passDist = Vector3.Distance(transform.position, newPos);
                var Ekdiff = hardnes * passDist * arrowSpec.PenetrationFactor;
                var currEk = Ek - Ekdiff;
                if (currEk < 0) {
                    var correctPassedDist = Ek / (hardnes * arrowSpec.PenetrationFactor);
                    var recalcPos = transform.position + dir * correctPassedDist;
                    transform.position = recalcPos;
                    break;
                } else {
                    transform.position = newPos;
                    Ek -= Ekdiff;
                }

                yield return Time.deltaTime;
            }
        }
        
        public void Hit(Vector3 hitPos, Vector3 tipPos, ArrowHitable hitable) {
            var hardnes = hitable.Hardnes;
            var velocity = rigid.velocity;
            var direction = velocity.normalized;

            var layerMask = (1 << hitableLayerIndex);
            var hits = Physics.RaycastAll(hitPos + direction * 10, -direction, 10, layerMask);

            RaycastHit? equalHit = null;

            foreach (var hit in hits)
                if (hit.collider.gameObject.Equals(hitable.gameObject))
                    equalHit = hit;

            if (equalHit == null)
                return;

            var reverceHit = (RaycastHit)equalHit;

            if (reverceHit.collider.TryGetComponent(out ArrowPushable pushable)) {
                pushable.Push(Vector3.zero, rigid.velocity);
            }
            
            var outsidePos = reverceHit.point;
            var arrowNotchTipDiff = tip.position - transform.position;
            transform.position = hitPos - arrowNotchTipDiff;
            
            if (hitable.IsEndless)
                pendingRoutine = StartCoroutine(GoTrowEndlessObject(hitPos, outsidePos, hitable));
            else
                pendingRoutine = StartCoroutine(GoThrowObject(hitPos, outsidePos, hitable));
        }

        private void Recoshet(Vector3 arrowDir, Vector3 hitDir, float bounce) {
            var newDir = Vector3.Reflect(arrowDir, hitDir);
            var velocxity = rigid.velocity.magnitude;
            rigid.velocity = newDir * velocxity * bounce;
            rigid.rotation = Quaternion.LookRotation(rigid.velocity, transform.up);
        }
    }
}
