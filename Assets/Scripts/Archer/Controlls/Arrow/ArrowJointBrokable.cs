using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archer.Controlls.ArrowHitableControlls
{
    public class ArrowJointBrokable :MonoBehaviour
    {
        private Joint joint;

        private void Start()
        {
            joint = GetComponent<Joint>();
        }

        public void Hit()
        {

        }
    }
}
