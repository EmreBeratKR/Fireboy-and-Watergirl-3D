using UnityEngine;
using Random = UnityEngine.Random;

public class SubWindParticle : MonoBehaviour
{    
    private TrailRenderer trailRenderer;
    public Vector3 velocity;


    private void Start()
    {
        trailRenderer = this.GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        transform.localPosition += velocity * Time.deltaTime;

        Shrink();
    }

    public void Clear()
    {
        trailRenderer.Clear();
        trailRenderer.widthMultiplier = 1;
    }

    public void Shrink()
    {
        trailRenderer.widthMultiplier -= 1f * Time.deltaTime;
    }
}
