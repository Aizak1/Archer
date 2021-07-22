using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoldButtonEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    [SerializeField]
    private UnityEvent unityEvent;

    [SerializeField]
    private RawImage rawImage;

    [SerializeField]
    private Color holdColor;

    private Color originalColor;

    private bool isHold = false;

    private void Awake() {
        originalColor = rawImage.color;
    }

    public void OnPointerDown(PointerEventData eventData) {
        rawImage.color = holdColor;
        isHold = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        rawImage.color = originalColor;
        isHold = false;
    }

    private void Update() {
        if (!isHold) {
            return;
        }
        unityEvent.Invoke();
    }
}
