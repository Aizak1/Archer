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
    private Material freezeMaterial;
    private const string SHADER_FREEZE_FIELD = "_Cutoff";
    private const float FREEZE_LERP_MIN = -0.3f;
    private float unfreezeTime;
    private MeshRenderer meshRenderer;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
        freezeMaterial = meshRenderer.sharedMaterials[meshRenderer.sharedMaterials.Length - 1];
        freezeMaterial.SetFloat(SHADER_FREEZE_FIELD, -1);
        originalAnimatorSpeed = animator.speed;
    }

    public void ProcessHit(Arrow arrow) {
        if (arrow.arrowType != ArrowType.Freeze) {
            return;
        }
        freezeMaterial.SetFloat(SHADER_FREEZE_FIELD, 1);
        animator.speed = 0;
        unfreezeTime = Time.time + freezeTime;
    }

    private void Update() {
        if(animator.speed != 0) {
            return;
        }

        var percent = 1 - (unfreezeTime - Time.time) / freezeTime;
        float freezeValue = Mathf.Lerp(1, FREEZE_LERP_MIN, percent);
        freezeMaterial.SetFloat(SHADER_FREEZE_FIELD, freezeValue);

        if(Time.time >= unfreezeTime) {
            animator.speed = originalAnimatorSpeed;
            freezeMaterial.SetFloat(SHADER_FREEZE_FIELD, -1);
        }

    }
}
