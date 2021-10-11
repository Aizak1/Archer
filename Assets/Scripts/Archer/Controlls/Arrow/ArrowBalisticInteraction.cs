using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Archer.Controlls.ArrowHitableControlls;
using Archer.Specs.Hitable;



namespace Archer.Controlls.ArrowControlls {
    //[RequireComponent(typeof(ArrowHitable))]
    [RequireComponent(typeof(Collider))]
    public class ArrowBalisticInteraction : MonoBehaviour {
        [SerializeField] private HitableSpec hitableSpec;
        [SerializeField] private Rigidbody rigid;
        
        //private ArrowHitable arrowHitable;
        private Collider collider;
        
        private void Start() {
            //arrowHitable = GetComponent<ArrowHitable>();
            collider = GetComponent<Collider>();
        }

        public void PerformBalisticHit(ArrowController arrow) {
            Debug.Log(gameObject.name);
            arrow.transform.SetParent(transform);
            var tipTran = arrow.TipTran;
            var arrowRigit = arrow.ArrowRigid;
            var enterPointPos = tipTran.position;
            //Physics.RaycastAll();
        }

    }
}
