using arrow;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ui;
using portal;
using UnityEngine.Events;

namespace bow {
    public class Bow : MonoBehaviour {
        
        [SerializeField] private GameObject arrowPlacementPoint;
        [SerializeField] private GameObject bowRotationPivot;

        [SerializeField] private float minBowRotationAngle;
        [SerializeField] private float maxBowRotationAngle;
        
       
        [SerializeField] private ArrowResource arrowResource;
        private ArrowSpawnObject arrowToInstantiate;

        [HideInInspector]
        public float pullAmount;
        [HideInInspector]
        public Arrow instantiatedArrow;
        
    
        [HideInInspector]public int shotsCount;

        public UnityAction OnStartPull;
        public UnityAction OnPull;
        public UnityAction OnEndPull;
        
        

        private void Start() {
            shotsCount = 0;
            arrowToInstantiate = arrowResource.ArrowPrefabs[0];
            
        }
        

        public void StartPull()
        {
            var pos = arrowPlacementPoint.transform.position;
            var rot = arrowPlacementPoint.transform.rotation;
            var parent = arrowPlacementPoint.transform;
                
            var arrowGameObject = Instantiate(arrowToInstantiate, pos, rot, parent);
            instantiatedArrow = arrowGameObject.CurrentArrow;
            instantiatedArrow.enabled = false;
            OnStartPull?.Invoke();
        }

        public void Pull(Vector3 startTouchPosition, Vector3 pullPosition, Vector3 maxPull, Vector3 minPull)
        {
            var targetPosition = (startTouchPosition - pullPosition).normalized;

            float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;
            angle = Mathf.Clamp(angle, minBowRotationAngle, maxBowRotationAngle);

            bowRotationPivot.transform.rotation = Quaternion.AngleAxis(angle, Vector3.left);

            pullAmount = CalculatePullAmount(pullPosition,startTouchPosition,maxPull,minPull);
            OnPull?.Invoke();
        }

        public void EndPull()
        {
            if (instantiatedArrow == null)
            {
                return;
            }
            instantiatedArrow.enabled = true;
            var arrowTransform = instantiatedArrow.transform;
            arrowTransform.parent = null;

            var x = bowRotationPivot.transform.parent.transform.position.x;
            var y = arrowTransform.position.y;
            var z = arrowTransform.position.z;

            arrowTransform.position.Set(x,y,z);
            arrowTransform.rotation.Set(arrowTransform.rotation.x,0,0,arrowTransform.rotation.w);

            var direction = instantiatedArrow.transform.forward;
            var velocity = instantiatedArrow.Speed * pullAmount * direction;
            instantiatedArrow.Release(velocity);

              
            shotsCount++;
            pullAmount = 0;
            instantiatedArrow = null;
            OnEndPull?.Invoke();
        }

        private float CalculatePullAmount(Vector3 pullPosition, Vector3 startTouchPosition,Vector3 maxPull,Vector3 minPull) 
        {
            var pullVector = pullPosition - startTouchPosition;
            var maxPullVector = maxPull - minPull;
            float pullAmount = pullVector.magnitude / maxPullVector.magnitude;

            return Mathf.Clamp(pullAmount, 0, 1);
        }
    }
}