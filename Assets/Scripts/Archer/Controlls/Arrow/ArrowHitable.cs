using UnityEngine;

namespace Archer.Controlls.ArrowHitableControlls {
    [RequireComponent(typeof(Collider))]
    public class ArrowHitable : MonoBehaviour
    {
        [Header("Params")]
        [SerializeField] private float hardnes;
        [SerializeField] private float bounce;
        [Header("Options")]
        [SerializeField] private bool isEndless;

        public float Hardnes => hardnes;
        public float Bounce => bounce;

        public bool IsEndless => isEndless;
    }
}
