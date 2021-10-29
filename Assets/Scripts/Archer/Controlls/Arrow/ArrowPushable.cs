using UnityEngine;

namespace Archer.Controlls.ArrowHitableControlls {
    public class ArrowPushable : MonoBehaviour {

        [SerializeField] private float kinematicAbsorbation;
        [SerializeField] private Vector3 massCenter;
        [SerializeField] private Rigidbody rigid;

        private Vector3 restorPos;

        private void Start() {
            rigid.isKinematic = false;
            rigid.useGravity = true;
            restorPos = transform.position;
        }

        public void Push(Vector3 hitPos, Vector3 force)
        {
            rigid.useGravity = true;
            rigid.isKinematic = false;
            rigid.AddForce(force, ForceMode.Impulse);
        }
    }
}
