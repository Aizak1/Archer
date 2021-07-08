using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowController : MonoBehaviour
{
    [SerializeField]
    private Transform start;
    [SerializeField]
    private Transform max;

    [SerializeField]
    private GameObject pivot;

    [SerializeField]
    private GameObject arrow;

    public float pullAmount;
    private Vector3 startTouchPosition;

    private void Update()
    {
        if(Input.touchCount <= 0) {
            return;
        }

        if (Input.touches[0].phase == TouchPhase.Began) {

            startTouchPosition = Camera.main.ScreenToViewportPoint(Input.touches[0].position);
            startTouchPosition.z = max.position.z;

            var pos = pivot.transform.position;
            Instantiate(arrow, pos, pivot.transform.rotation, pivot.transform);
        }

        if(Input.touches[0].phase == TouchPhase.Moved) {

            Vector3 pullPosition = Camera.main.ScreenToViewportPoint(Input.touches[0].position);
            pullPosition.z = max.position.z;

            if (pullPosition.x > startTouchPosition.x) {
                return;
            }

            var targetPosition = (startTouchPosition - pullPosition).normalized;

            float angle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;
            angle = Mathf.Clamp(angle, -90f, 90f);

            transform.parent.transform.rotation = Quaternion.AngleAxis(angle, Vector3.left);

            pullAmount = CalculatePullAmount(pullPosition);

        }

        if (Input.touches[0].phase == TouchPhase.Ended) {
            pullAmount = 0;
            pivot.transform.localPosition = start.localPosition;
            startTouchPosition = Vector3.zero;
        }


    }

    private float CalculatePullAmount(Vector3 pullPosition) {
        var pullVector = pullPosition - startTouchPosition;
        var maxPullVector = max.position - start.position;
        float maxLength = maxPullVector.magnitude;
        float pullAmount = pullVector.magnitude / maxLength;

        return Mathf.Clamp(pullAmount, 0, 1);
    }
}
