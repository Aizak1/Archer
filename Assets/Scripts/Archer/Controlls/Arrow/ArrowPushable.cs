using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archer.Controlls.ArrowHitableControlls {
    public class ArrowPushable :MonoBehaviour {

        [SerializeField] private float kinematicAbsorbation;
        [SerializeField] private Vector3 massCenter;
        [SerializeField] private Rigidbody rigid;

        private Vector3 restorPos;

        private void Start() {
            rigid.isKinematic = true;
            rigid.useGravity = false;
            restorPos = transform.position;
        }

        public void Push(Vector3 hitPos, Vector3 arrowVelocity)
        {
            rigid.useGravity = true;
            rigid.isKinematic = false;
            rigid.velocity = arrowVelocity * (1 - kinematicAbsorbation);
        }

        // Debug
        [ContextMenu("Reset")]
        private void Reset()
        {
            rigid.isKinematic = true;
            rigid.useGravity = false;
            rigid.velocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.position = restorPos;
        }
    }
}
