using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class AnimatedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private string onEnterKey = "OnEnter";
    private string onExitKey = "OnExit";
    private Animator animator;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    private void ResetTrigers() {
        animator.ResetTrigger(onEnterKey);
        animator.ResetTrigger(onExitKey);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        ResetTrigers();
        animator.SetTrigger(onEnterKey);
    }

    public void OnPointerExit(PointerEventData eventData) {
        ResetTrigers();
        animator.SetTrigger(onExitKey);
    }
}
