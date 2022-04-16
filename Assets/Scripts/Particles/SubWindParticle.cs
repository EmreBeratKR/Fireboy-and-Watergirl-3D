using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(ParticleSystem))]
public class SubWindParticle : MonoBehaviour
{
    private new ParticleSystem particleSystem;
    
    public Vector3 velocity;

    private void Start()
    {
        particleSystem = this.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        transform.localPosition += velocity * Time.deltaTime;
    }


    public void Randomize()
    {
        var main = particleSystem.main;

        main.startSize = Random.Range(0.2f, 1f);
    }
}
