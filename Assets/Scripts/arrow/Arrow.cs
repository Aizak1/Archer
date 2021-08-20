using hittable;
using portal;
using ricochet;
using bow;
using player;
using UnityEngine;

namespace arrow {

    public enum ArrowType {
        Wooden,
        Double,
        Fast,
        Slow,
        Portal,
        Freeze,
        Fire
    }

    public class Arrow : MonoBehaviour {

        public ArrowType arrowType;

        public float speed;
        public bool isInAir;

        [SerializeField]
        public Transform tip;
        [SerializeField]
        private new Rigidbody rigidbody;

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
        [SerializeField]
        private GameObject fireVfx;

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
        private PortalArrow portalArrow;
        [HideInInspector]
        public TrailRenderer trailRenderer;
        [HideInInspector]
        public float trailTime;
        [HideInInspector]
        public bool isTeleporting;

        private BowController bowController;

        private void Awake() {
            lastTipPosition = tip.transform.position;
            trailRenderer = GetComponentInChildren<TrailRenderer>();
            trailTime = trailRenderer.time;
            trailRenderer.enabled = false;
            isTeleporting = false;
        }

        private void Update() {
            if (!isInAir) {
                return;
            }

            if (rigidbody.velocity == Vector3.zero) {
                return;
            }

            transform.rotation = Quaternion.LookRotation(rigidbody.velocity, transform.up);

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
                    GetComponent<Collider>().enabled = false;
                    isInAir = false;

                    rigidbody.useGravity = false;
                    rigidbody.isKinematic = true;

                    audioSource.PlayOneShot(impactSound);

                    var parent = new GameObject();
                    parent.transform.position = hit.collider.gameObject.transform.position;
                    parent.transform.rotation = hit.collider.gameObject.transform.rotation;

                    parent.transform.parent = hit.collider.gameObject.transform;
                    transform.parent = parent.transform;

                    if (portalArrow) {
                        CreatePortal(hit);
                        Destroy(gameObject);
                        return;
                    }

                    if (hitVfx) {
                        Instantiate(hitVfx, hit.point, Quaternion.LookRotation(hit.normal));
                    }

                    var hittable = hit.collider.GetComponent<Hittable>();
                    var freezable = hit.collider.GetComponent<FreezableObject>();
                    var burnable = hit.collider.GetComponent<BurnableObject>();
                    var player = hit.collider.GetComponent<Player>();

                    if (hittable) {
                        hittable.ProcessHit(this, hit);
                    } else if (freezable) {
                        freezable.ProcessHit(this);
                    } else if (burnable) {
                        burnable.ProcessHit(this);
                        if (fireVfx) {
                            fireVfx.SetActive(true);
                        }
                    } else if (player) {
                        player.ProcessHit(hit);

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
                        portal.StartPortalTravelling(GetComponent<Collider>());
                    }

                    var surface = hit.collider.GetComponent<RicochetSurface>();
                    if (surface) {
                        surface.Richochet(this);
                    }
                }
            }
            lastTipPosition = tip.position;

            if (Time.time >= splitTime && isSplit && !isTeleporting) {
                Split(angleBetweenSplitArrows, splitArrowsAmount);
                Instantiate(splitVfx, transform.position, Quaternion.identity);
            }
        }

        private void CreatePortal(RaycastHit hit) {
            var position = hit.point;
            var normal = hit.normal;
            normal.x = 0;
            var rotation = Quaternion.LookRotation(normal);
            GameObject portal;

            var parent = new GameObject();
            parent.transform.position = hit.collider.gameObject.transform.position;
            parent.transform.rotation = hit.collider.gameObject.transform.rotation;

            parent.transform.parent = hit.collider.gameObject.transform;

            if (portalArrow.isBlue) {

                var existPortal = GameObject.FindGameObjectWithTag(Portal.BLUE_PORTAL_TAG);
                if (existPortal) {
                    existPortal.GetComponent<Portal>().Close();
                }
                portal = Instantiate(portalArrow.bluePortal, position, rotation, parent.transform);

            } else {

                var existsPortal = GameObject.FindGameObjectWithTag(Portal.ORANGE_PORTAL_TAG);
                if (existsPortal) {
                    existsPortal.GetComponent<Portal>().Close();
                }
                portal = Instantiate(portalArrow.orangePortal, position, rotation, parent.transform);
            }

            portal.GetComponentInChildren<Portal>().Open();
            var offset = portal.transform.forward.normalized / Portal.PORTAL_SPAWN_OFFSET;
            offset.x = 0;
            portal.transform.position += offset;
        }

        private void Split(float angleBetweenSplitArrows, int splitArrowsAmount) {

            float angle = -angleBetweenSplitArrows * (splitArrowsAmount - 1) / 2;
            Arrow instantiatedArrow = this;

            for (int i = 0; i < splitArrowsAmount; i++) {
                instantiatedArrow = Instantiate(this, transform.position, transform.rotation);
                var velocity = rigidbody.velocity;

                float radAngle = angle * Mathf.Deg2Rad;

                float newY = Mathf.Sin(radAngle) * velocity.z + Mathf.Cos(radAngle) * velocity.y;
                float newZ = Mathf.Cos(radAngle) * velocity.z - Mathf.Sin(radAngle) * velocity.y;

                var newVelocity = new Vector3(velocity.x, newY, newZ);

                instantiatedArrow.Release(newVelocity,false,bowController);

                angle += angleBetweenSplitArrows;
            }

            instantiatedArrow.audioSource.PlayOneShot(splitSound);
            Destroy(gameObject);
        }

        public void Release(Vector3 velocity, bool isSplit, BowController bowController) {

            this.bowController = bowController;

            trailRenderer.enabled = true;
            isInAir = true;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
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
            if(transform.parent != null) {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}