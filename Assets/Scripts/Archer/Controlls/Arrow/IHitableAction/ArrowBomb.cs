using System.Text;
using System.Collections;
using UnityEngine;

namespace Archer.Controlls.IHitableAction {
    public class ArrowBomb : MonoBehaviour, IHitable {
        [SerializeField] private Transform explosionSphere;
        [SerializeField] private float explosionDelay;
        [SerializeField] private float explosionRadius;
        [SerializeField] private float explosionForce;
        
        private Coroutine pendingRotineRef;
        
        private int oclusionLayerIndex;

        private Vector3 resetPos;
        private Quaternion resetRot;

        private void Start()
        {
            resetPos = transform.position;
            resetRot = transform.rotation;
            oclusionLayerIndex = LayerMask.NameToLayer("OclusionColiders");
        }
        
        public void HitAction()
        {
            if (pendingRotineRef == null) {
                pendingRotineRef = StartCoroutine(CountDown());
            }
        }

        private IEnumerator CountDown()
        {
            var startTime = 0f;
            while(startTime < explosionDelay) {
                startTime += Time.deltaTime;
                yield return Time.deltaTime;
            }
            StartExplode();
            pendingRotineRef = null;
        }

        private void StartExplode()
        {
            VisualisateExplosion();
            var colidsions = GetTouchedByExplosion();
            var bombPos = transform.position;

            foreach (var colider in colidsions) {

                if (colider.TryGetComponent(out Rigidbody rigid)) {
                    var pos = rigid.transform.position;
                    var distance = Vector3.Distance(bombPos, pos);
                    var factor = distance / explosionRadius;
                    var aplyforce = factor * explosionForce;
                    var direction = (pos - bombPos).normalized;
                    var force = direction * explosionForce;
                    rigid.useGravity = true;
                    rigid.isKinematic = false;
                    rigid.AddForce(force, ForceMode.Impulse);
                }
            }
        }

        private void VisualisateExplosion()
        {
            explosionSphere.transform.localScale = Vector3.one * 10;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }

        private Collider[] GetTouchedByExplosion()
        {
            var layerMask = (1 << oclusionLayerIndex);
            var sb = new StringBuilder();
            var hits = Physics.OverlapSphere(transform.position, explosionRadius, layerMask, QueryTriggerInteraction.Ignore);
            foreach (var hit in hits) {
                sb.AppendLine(hit.name);
            }
            
            return hits;
        }

        [ContextMenu("Explode")]
        private void DebugExplode()
        {
            HitAction();
        }

        [ContextMenu("Reset")]
        private void DebugReset()
        {
            transform.position = resetPos;
            transform.rotation = resetRot;
            explosionSphere.transform.localScale = Vector3.one * 0.1f;
        }
    }
}
