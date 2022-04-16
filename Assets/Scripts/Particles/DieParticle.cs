using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DieParticle : MonoBehaviour
{
    private void Start()
    {
        var system = this.GetComponent<ParticleSystem>();
        
        Destroy(gameObject, system.main.duration);
    }
}
