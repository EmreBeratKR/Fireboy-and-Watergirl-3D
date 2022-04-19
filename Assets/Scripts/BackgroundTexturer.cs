using UnityEngine;

public class BackgroundTexturer : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField, Range(10f, 100f)] private float materialScale;
    private const float textureConstant = 0.3f;


    private void OnValidate()
    {
        material.mainTextureScale = Vector2.one * materialScale * textureConstant;
    }
}
