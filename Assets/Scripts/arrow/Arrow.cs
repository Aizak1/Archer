using hittable;
using portal;
using bow;
using UnityEngine;
using homing;

namespace arrow {

    public enum ArrowType {
        Wooden,
        Double,
        Fast,
        Slow,
        Portal,
        Freeze,
        Fire,
        Homing
    }

    public class Arrow : MonoBehaviour {

        public ArrowType arrowType;

        public float speed;

        [SerializeField]
        public Transform tip;
        [SerializeField]
        public new Rigidbody rigidbody;

        [SerializeField]
        private int splitArrowsAmount;

        [SerializeField]
        private float timeBeforeSplit;

        [SerializeField]
        private float angleBetweenSplitArrows;

        [SerializeField]
        private GameObject mainVfx;
        [SerializeField]
        private ParticleSystem hitVfx;
        [SerializeField]
        private ParticleSystem splitVfx;

        private const float VFX_LIFE_AFTER_HIT = 0.3f;

        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip impactSound;
        [SerializeField]
        private AudioClip splitSound;

        private float splitTime;
        private bool isSplit;

        private Vector3 lastTipPosition;
        private const float TIP_POS_ACCURACY = 1;
        private const string RAYCAST_LAYER = "Default";

        [SerializeField]
        public TrailRenderer trailRenderer;
        [HideInInspector]
        public bool isTeleporting;

        private BowController bowController;

        [SerializeField]
        private new Collider collider;

        private HomingArrow homingArrowComponent;

        private void Awake() {
            lastTipPosition = tip.transform.position;
            trailRenderer.enabled = false;
            isTeleporting = false;

            if(arrowType == ArrowType.Homing) {
                homingArrowComponent = GetComponent<HomingArrow>();
            }

        }

        private void Update() {
            if(arrowType == ArrowType.Homing) {
                var lookPos = homingArrowComponent.currentPoint.position - transform.position;
                if(lookPos != Vector3.zero) {
                    transform.rotation = Quaternion.LookRotation(lookPos, transform.up);
                }
            } else {
                transform.rotation = Quaternion.LookRotation(rigidbody.velocity, transform.up);
            }


            var tipDistance = (lastTipPosition - tip.position).magnitude;
            var mask = LayerMask.GetMask(RAYCAST_LAYER);
            if (tipDistance < TIP_POS_ACCURACY &&
                Physics.Linecast(lastTipPosition, tip.position, out RaycastHit hit, mask)) {

                if (!hit.collider.isTrigger) {

                    enabled = false;

                    trailRenderer.enabled = false;
                    if (mainVfx != null) {
                        Destroy(mainVfx, VFX_LIFE_AFTER_HIT);
                    }

                    rigidbody.Sleep();
                    collider.enabled = false;

                    rigidbody.useGravity = false;
                    rigidbody.isKinematic = true;

                    audioSource.PlayOneShot(impactSound);

                    var parent = new GameObject();
                    parent.transform.position = hit.collider.gameObject.transform.position;
                    parent.transform.rotation = hit.collider.gameObject.transform.rotation;
                    parent.transform.parent = hit.collider.gameObject.transform;

                    transform.parent = parent.transform;

                    if (arrowType == ArrowType.Portal) {
                        bowController.portalController.CreatePortal(hit);
                        Destroy(gameObject);
                        return;
                    }

                    if (hitVfx) {
                        Instantiate(hitVfx, hit.point, Quaternion.LookRotation(hit.normal));
                    }

                    var hittable = hit.collider.GetComponent<Hittable>();

                    if (hittable) {
                        hittable.ProcessHit(this, hit);
                    } else {
                        bowController.arrowsOnLevel.Enqueue(gameObject);
                    }

                    if (bowController.arrowsOnLevel.Count > BowController.MAX_ARROWS_COUNT) {
                        var arrow = bowController.arrowsOnLevel.Dequeue();
                        Destroy(arrow);
                    }

                    trailRenderer.enabled = false;

                    return;

                } else {

                    var portal = hit.collider.GetComponent<Portal>();
                    if (portal) {
                        trailRenderer.Clear();
                        trailRenderer.enabled = false;
                        isTeleporting = true;
                        portal.StartPortalTravelling(collider);
                    }
                }
            }
            lastTipPosition = tip.position;

            if (Time.time >= splitTime && isSplit && !isTeleporting) {
                Instantiate(splitVfx, transform.position, Quaternion.identity);
                Split(angleBetweenSplitArrows, splitArrowsAmount);
            }

            if(arrowType == ArrowType.Homing) {
                float accuracy = 0.1f;
                var pointPos = homingArrowComponent.currentPoint.position;
                if ((transform.position - pointPos).magnitude < accuracy) {
                    homingArrowComponent.ChangeCurrentPoint();
                } else {
                    var speed = HomingArrow.SPEED * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, pointPos, speed);
                }

            }
        }

        private void Split(float angleBetweenSplitArrows, int splitArrowsAmount) {

            float angle = -angleBetweenSplitArrows * (splitArrowsAmount - 1) / 2;
            Arrow instantiatedArrow = this;

            for (int i = 0; i < splitArrowsAmount; i++) {
                instantiatedArrow = Instantiate(this, transform.position, transform.rotation);
                Vector3 velocity = rigidbody.velocity;

                float radAngle = angle * Mathf.Deg2Rad;

                float newY = Mathf.Sin(radAngle) * velocity.z + Mathf.Cos(radAngle) * velocity.y;
                float newZ = Mathf.Cos(radAngle) * velocity.z - Mathf.Sin(radAngle) * velocity.y;

                Vector3 newVelocity = new Vector3(velocity.x, newY, newZ);

                instantiatedArrow.Release(newVelocity, false, bowController);

                angle += angleBetweenSplitArrows;
            }

            instantiatedArrow.audioSource.PlayOneShot(splitSound);
            Destroy(gameObject);
        }

        public void Release(Vector3 velocity, bool isSplit, BowController bowController) {

            this.bowController = bowController;

            trailRenderer.enabled = true;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;

            if (arrowType == ArrowType.Homing) {
                velocity = Vector3.zero;
                rigidbody.useGravity = false;
            }

            rigidbody.velocity = velocity;

            if (splitArrowsAmount <= 1) {
                isSplit = false;
            }

            this.isSplit = isSplit;

            if (this.isSplit) {
                splitTime = Time.time + timeBeforeSplit;
            }

        }

        private void OnDestroy() {
            if (transform.parent != null) {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}