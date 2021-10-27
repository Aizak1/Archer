using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Archer.DataOperator {
    public class DataOperator : MonoBehaviour {
        
    }



    [Serializable]
    public struct Player {
        public Player(int id) {
            this.id = id;
        }

        private int id;
    }
}
