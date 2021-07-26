using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using hittable;
using arrow;

[RequireComponent(typeof(Hittable))]
public class PatrolingEnemy : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    private float originalAnimatorSpeed;
    [SerializeField]
    private float freezeTime;
    private float unfreezeTime;

    public void ProcessHit(Arrow arrow) {
        if (arrow.arrowType != ArrowType.Freeze) {
            return;
        }
        originalAnimatorSpeed = animator.speed;
        animator.speed = 0;
        unfreezeTime = Time.time + freezeTime;
    }

    private void Update() {
        if(animator.speed != 0) {
            return;
        }

        if(Time.time >= unfreezeTime) {
            animator.speed = originalAnimatorSpeed;
        }

    }
}
