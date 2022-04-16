using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : Singleton<ParticleManager>
{
    [SerializeField] private DieParticles dieParticles;


    public static void EmitDieParticle(Vector3 position, Element element)
    {
        var particle = (element == Element.Fire) ? Instance.dieParticles.fireboy : Instance.dieParticles.watergirl;

        Instantiate(particle, position, Quaternion.identity, Instance.transform);
    }
}

[Serializable]
internal struct DieParticles
{
    public GameObject fireboy;
    public GameObject watergirl;
}