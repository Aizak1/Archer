using UnityEngine;

namespace Archer.Controlls.ArrowHitableControlls {
    [RequireComponent(typeof(Collider))]
    public class ArrowHitable : MonoBehaviour
    {
        [SerializeField] private float hardnes;
        [SerializeField] private float bounce;

        public float Hardnes => hardnes;
        public float Bounce => bounce;
    }
}
