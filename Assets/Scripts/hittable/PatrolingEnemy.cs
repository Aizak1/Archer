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
    private const string SHADER_FREEZE_FIELD = "_IceSlider";
    private const float FREEZE_MIN = 0f;
    private float unfreezeTime;
    private MeshRenderer meshRenderer;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
        freezeMaterial = meshRenderer.sharedMaterials[0];
        freezeMaterial.SetFloat(SHADER_FREEZE_FIELD, FREEZE_MIN);
        originalAnimatorSpeed = animator.speed;
    }

    public void ProcessHit(Arrow arrow) {
        if (arrow.arrowType != ArrowType.Freeze) {
            return;
        }
        animator.speed = 0;
        unfreezeTime = Time.time + freezeTime;
    }

    private void Update() {
        if(animator.speed != 0) {
            return;
        }

        var percent = 1 - (unfreezeTime - Time.time) / freezeTime;
        float freezeValue = Mathf.Lerp(1, FREEZE_MIN, percent);
        freezeMaterial.SetFloat(SHADER_FREEZE_FIELD, freezeValue);

        if(Time.time >= unfreezeTime) {
            animator.speed = originalAnimatorSpeed;
            freezeMaterial.SetFloat(SHADER_FREEZE_FIELD, FREEZE_MIN);
        }

    }

    private void OnDestroy() {
        freezeMaterial.SetFloat(SHADER_FREEZE_FIELD, FREEZE_MIN);
    }
}
