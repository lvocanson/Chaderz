using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightningDecal : MonoBehaviour
{
    [SerializeField]
    float _lifetime;

    [SerializeField]
    DecalProjector _projector;

    [SerializeField]
    AnimationCurve _opacityCurve;

    void Start()
    {
        StartCoroutine(DecalCoroutine());
    }

    private IEnumerator DecalCoroutine()
    {
        float timer = 0;
        while (timer < _lifetime)
        {
            _projector.fadeFactor = _opacityCurve.Evaluate(timer / _lifetime);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
