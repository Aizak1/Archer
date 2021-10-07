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
            rigid.isKinematic = false;
            rigid.useGravity = true;
            restorPos = transform.position;
        }

        public void Push(Vector3 hitPos, Vector3 force)
        {
            Debug.Log("Push");
            rigid.useGravity = true;
            rigid.isKinematic = false;
            rigid.AddForce(force, ForceMode.Impulse);
        }

        // Debug
        [ContextMenu("Reset")]
        private void Reset()
        {
            rigid.isKinematic = false;
            rigid.useGravity = true;
            rigid.velocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.position = restorPos;
        }

        [ContextMenu("Push")]
        private void DebugPush()
        {
            Push(Vector3.zero, Vector3.forward * 5000f);
        }

    }
}
