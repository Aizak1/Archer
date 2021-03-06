using UnityEngine;
using hittable;
using arrow;

[RequireComponent(typeof(Hittable))]
public class MovingTarget : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public void ProcessHit() {
        animator.speed = 0;
        Destroy(this);
    }

    private void OnDestroy() {
        Destroy(GetComponent<Hittable>());
    }
}
