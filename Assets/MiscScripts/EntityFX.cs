using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;
    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    [SerializeField] private Material ogMat;
    [SerializeField] private float flashDuration;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        ogMat = sr.material;
    }

    private IEnumerator FlashFX()
    {
        sr.material = hitMat;

        yield return new WaitForSeconds(flashDuration);

        sr.material = ogMat;
    }
}
