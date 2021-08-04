using hittable;
using portal;
using ricochet;
using UnityEngine;

namespace arrow {

    public enum ArrowType {
        Wooden,
        Double,
        Fast,
        Slow,
        Portal,
        Freeze,
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
        private ParticleSystem hitVfx;
        [SerializeField]
        private ParticleSystem splitVfx;

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
        }

        private void FixedUpdate() {
            if (!isInAir) {
                return;
            }

            if (rigidbody.velocity == Vector3.zero) {
                return;
            }

            var tipDistance = (lastTipPosition - tip.position).magnitude;
            var mask = LayerMask.GetMask(RAYCAST_LAYER);
            if (tipDistance < TIP_POS_ACCURACY &&
                Physics.Linecast(lastTipPosition, tip.position, out RaycastHit hit, mask)) {

                if (!hit.collider.isTrigger) {

                    trailRenderer.enabled = false;

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

                    if (portalArrow != null) {
                        CreatePortal(hit);
                        Destroy(gameObject);
                        return;
                    }

                    if(hitVfx != null) {
                        Instantiate(hitVfx, hit.point, Quaternion.LookRotation(hit.normal));
                    }

                    var hittable = hit.collider.GetComponent<Hittable>();
                    if (hittable != null) {
                        hittable.ProcessHit(this, hit);
                    }

                    trailRenderer.enabled = false;

                    return;
                } else {

                    var portal = hit.collider.GetComponent<Portal>();
                    if(portal != null) {
                        trailRenderer.Clear();
                        trailRenderer.enabled = false;
                        isTeleporting = true;
                        portal.StartPortalTravelling(GetComponent<Collider>());
                    }

                    var surface = hit.collider.GetComponent<RicochetSurface>();
                    if(surface != null) {
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
            var rotation = Quaternion.LookRotation(hit.normal);
            GameObject portal;
            if (portalArrow.isBlue) {

                var existPortal =
                    GameObject.FindGameObjectWithTag(Portal.BLUE_PORTAL_TAG);

                if (existPortal != null) {
                    existPortal.GetComponent<Portal>().Close();
                }

                portal = Instantiate(portalArrow.bluePortal, position, rotation);
            } else {
                var existsPortal =
                    GameObject.FindGameObjectWithTag(Portal.ORANGE_PORTAL_TAG);

                if (existsPortal != null) {
                    existsPortal.GetComponent<Portal>().Close();
                }

                portal = Instantiate(portalArrow.orangePortal, position, rotation);
            }
            portal.GetComponentInChildren<Portal>().Open();
            var offset = portal.transform.forward / Portal.PORTAL_SPAWN_OFFSET;
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

                instantiatedArrow.Release(newVelocity,false);

                angle += angleBetweenSplitArrows;
            }
            instantiatedArrow.audioSource.PlayOneShot(splitSound);
            Destroy(gameObject);
        }

        public void Release(Vector3 velocity, bool isSplit) {
            trailRenderer.enabled = true;
            isInAir = true;
            this.isSplit = isSplit;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
            rigidbody.velocity = velocity;

            if (splitArrowsAmount <= 1) {
                isSplit = false;
            }

            if (isSplit) {
                splitTime = Time.time + timeBeforeSplit;
            }
        }
    }
}