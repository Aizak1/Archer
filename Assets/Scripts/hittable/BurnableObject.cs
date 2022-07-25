using UnityEngine;
using arrow;
using arrow.behaviours;

namespace hittable {
    [RequireComponent(typeof(MeshRenderer),typeof(BurnCotroller))]
    public class BurnableObject : MonoBehaviour, IHittable{

        [SerializeField]private Material _burnMaterial;
        
        private bool _isBurning;
        private new MeshRenderer _renderer;
        private BurnCotroller _burnController;

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
            _burnController = GetComponent<BurnCotroller>();
            _burnController.enabled = false;
        }
        
        public void ProcessHit(Arrow arrow, RaycastHit hit) {

            if (_isBurning) {
                return;
            }
            
            foreach (var item in arrow.AdditionalArrowBehaviors)
            {
                if (item.BehaviorType != BehaviorType.Fire)
                {
                   continue;
                }
                
                Destroy(arrow.gameObject);

                _burnController.SetBurnStartTime(Time.time);
                _isBurning = true;

                _renderer.material = _burnMaterial;

                _burnController.enabled = true;
                return; 
            }

        }
    }
}