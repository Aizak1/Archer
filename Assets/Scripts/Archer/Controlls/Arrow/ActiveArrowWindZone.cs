using System.Collections.Generic;
using UnityEngine;
using Archer.Controlls.ArrowControlls;

namespace Archer.Controlls.ActiveArrowWindzoneController {
    public class ActiveArrowWindZone :MonoBehaviour {

        [SerializeField] private Vector3 halphExtend;
        [SerializeField] private Vector3 windDirection;
        [SerializeField] private float windForce;

        private int arrowHitableLayerIndex;

        private void Start() {
            arrowHitableLayerIndex = LayerMask.NameToLayer("Arrow");
        }

        private void FixedUpdate() {
            var flyingArrows = GetFlyingArrows();
            foreach (var arrow in flyingArrows) {
                if(arrow.TryGetComponent(out Rigidbody rigid)) {
                    var force = windDirection * windForce * Time.deltaTime;
                    rigid.AddForce(force, ForceMode.Impulse);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            var size = halphExtend * 2;
            var newPos = new Vector3(
                transform.position.x, transform.position.y + halphExtend.y, transform.position.z);
            Gizmos.DrawWireCube(newPos, size);
        }

        private List<ArrowController> GetFlyingArrows()
        {
            var layerMask = (1 << arrowHitableLayerIndex);
            var newPos = new Vector3(
                transform.position.x, transform.position.y + halphExtend.y, transform.position.z);
            var coliders = Physics.OverlapBox(newPos,
                halphExtend, Quaternion.identity, layerMask, QueryTriggerInteraction.Ignore);
            var arrowList = new List<ArrowController>();
            foreach (var colider in coliders) {
                if (colider.TryGetComponent(
                    out ArrowController arrowController) && arrowController.IsInAir) {
                    arrowList.Add(arrowController);
                }
            }

            return arrowList;
        }
    }
}
