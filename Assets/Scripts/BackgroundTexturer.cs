using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BackgroundTexturer : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField, Range(0f, 1f)] private float materialScale;
    private const float textureConstant = 0.075f;


    private void OnEnable() // change this to OnEnable() !!!
    {
        material.mainTextureScale = transform.lossyScale * materialScale * textureConstant;
    }
}
