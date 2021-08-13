using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnCotroller : MonoBehaviour
{
    [SerializeField] public ParticleSystem particleSystem = null;
    [SerializeField] private ParticleSystem.MinMaxCurve emisionCurve;
    [SerializeField] private MeshRenderer meshRenderer = null;
    [SerializeField] private Transform burnTransform = null;

    [Range(0,1)]
    [SerializeField] public float animationStage = 0f;
    [Range(0, 50)]
    [SerializeField] private int emisionMyltiplier = 0;

    [Header("FirsParticleSet")]
    [SerializeField] private bool isMoveEmitor;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 endPosition;

    private ParticleSystem.EmissionModule emisonModule;

    private void Start()
    {
        emisonModule = particleSystem.emission;
    }

    private void Update()
    {
        if (animationStage < 0.01f || animationStage > 0.99f)
        {
            particleSystem.Stop();
            if (!particleSystem.IsAlive())
                particleSystem.gameObject.SetActive(false);
        }
        else
        {
            particleSystem.gameObject.SetActive(true);
        }
        var valuemin = emisionCurve.curveMin.Evaluate(animationStage);
        var valuemax = emisionCurve.curveMax.Evaluate(animationStage);
        var random = UnityEngine.Random.Range(valuemin, valuemax);
        var randomeValue = (int)(emisionMyltiplier * random) + 1;
        meshRenderer.material.SetFloat("_FireRelativeheight", animationStage);
        emisonModule.rateOverTime = randomeValue * randomeValue;
        if (isMoveEmitor)
            particleSystem.transform.localPosition = Vector3.Lerp(startPosition, endPosition, animationStage);

        meshRenderer.material.SetVector("_Position", burnTransform.transform.position);
    }

    [ContextMenu("GetStartPosition")]
    private void GetStartPosition()
    {
        startPosition = particleSystem.transform.localPosition;
    }

    [ContextMenu("GetEndPosition")]
    private void GetEndPosition()
    {
        endPosition = particleSystem.transform.localPosition;
    }
}
