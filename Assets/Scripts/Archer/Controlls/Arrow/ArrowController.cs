using Archer.Specs.Arrow;
using UnityEngine;
using Archer.Controlls.ArrowHitableControlls;
using Archer.Specs.Rigid;
using Archer.Extension.Rigid;

namespace Archer.Controlls.ArrowControlls {
    public class ArrowController : MonoBehaviour
    {
        [SerializeField] private ArrowSpec arrowSpec;
        [SerializeField] private RigidSpec rigidSpec;
        [SerializeField] private Rigidbody rigid;
        [SerializeField] private BoxCollider colider;
        [SerializeField] private TrailRenderer trail;
        [SerializeField] private Transform tip;

        private Vector3 prevTipPos;
        private int hitableLayerIndex;
        private bool wasShoot;
        private bool isInAir;
        private Transform arrowPool;

        public bool IsInAir => isInAir;
        public bool WasShoot => wasShoot;

        public Transform TipTran => tip;
        public Rigidbody ArrowRigid => rigid;
        public ArrowSpec ArrowSpec => arrowSpec;
        public RigidSpec RigidSpec => rigidSpec;

        private void Start() {
            hitableLayerIndex = LayerMask.NameToLayer("ArrowHittable");
            prevTipPos = tip.position;
        }

        private void FixedUpdate() {

            if (!isInAir)
                return;

            if (isInAir && rigid != null && rigid.velocity != Vector3.zero)
                rigid.rotation = Quaternion.LookRotation(rigid.velocity, transform.up);
            var layerMask = (1 << hitableLayerIndex);
            if (Physics.Linecast(
               prevTipPos, tip.position, out RaycastHit hit, layerMask)) {
                var hitable = hit.collider.GetComponent<ArrowHitable>();
                var hitPoint = hit.point;
                TryPerformHit(hitPoint, hitable);
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
            var fixDirection = new Vector3(0, direction.y, direction.z).normalized;
            rigid.AddForce(fixDirection * force, ForceMode.Impulse);
            if (arrowPool != null)
                transform.SetParent(arrowPool);
        }

        public void Split() {
            if (isInAir && arrowSpec.IsSplitable) {
                Split(arrowSpec.SplitAngle, arrowSpec.SplitCount);
            }
        }

        private void Split(float angleBetweenSplitArrows, int splitArrowsAmount) {
            if (!isInAir)
                return;
            float angle =  -angleBetweenSplitArrows * (splitArrowsAmount - 1) / 2;
            var arrowAngle = transform.rotation.eulerAngles.x;
            var speed = rigid.velocity.magnitude;
            var impulse = rigidSpec.Mass * speed;
            for (int i = 0; i < splitArrowsAmount; i++) {
                var instantiatedArrow = Instantiate(this, transform.position, transform.rotation);
                var partialImpulce = impulse / splitArrowsAmount;
                instantiatedArrow.transform.rotation = Quaternion.Euler(arrowAngle + angle, 0, 0);
                instantiatedArrow.Release(partialImpulce);

                angle += angleBetweenSplitArrows;
            }
            Destroy(gameObject);
        }

        public void ToggleIsInObject(bool isInObject) {
            isInAir = !isInObject;
            if (!isInAir) {
                if (rigid != null) {
                    rigid.velocity = Vector3.zero;
                    rigid.useGravity = false;
                    Destroy(rigid);
                }
            } else {
                rigid = gameObject.AddComponent<Rigidbody>();
                rigidSpec.ApplyToRigid(rigid);
                rigid.velocity = Vector3.zero;
                rigid.useGravity = true;
                rigid.isKinematic = false;
                if (arrowPool != null)
                    transform.SetParent(arrowPool);
            }
        }

        public void RemoveControl() {
            Destroy(trail);
            Destroy(colider);
            if (rigid != null)
                Destroy(rigid);
        }

        public void SetArrowPool(Transform arrowContainer) {
            arrowPool = arrowContainer;
        }

        public void TryPerformHit(Vector3 hitPos, ArrowHitable hitable) {
            if (!isInAir || rigid == null)
                return;

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

            hitable.PerformHit(this);
        }
    }
}
