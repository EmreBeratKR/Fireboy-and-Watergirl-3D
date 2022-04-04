using System;
using UnityEngine;

[Serializable]
public struct AudioContainer
{
    [field:SerializeField] public Ost ost { get; private set; }
    [field:SerializeField] public Sfx sfx { get; private set; }



    [Serializable]
    public struct Ost
    {
        [field:SerializeField] public AudioSource normal { get; private set; }
        [field:SerializeField] public AudioSource epic { get; private set; }
        [field:SerializeField] public AudioSource normalFinish { get; private set; }
        [field:SerializeField] public AudioSource epicFinish { get; private set; }
        [field:SerializeField] public AudioSource gameover { get; private set; }
    }

    [Serializable]
    public struct Sfx
    {
        [field:SerializeField] public CharacterSfx fireboy { get; private set; }
        [field:SerializeField] public CharacterSfx watergirl { get; private set; }
        [field:SerializeField] public Ambiance ambiance { get; private set; }
    }

    [Serializable]
    public struct CharacterSfx
    {
        [field:SerializeField] public AudioSource jump { get; private set; }
        [field:SerializeField] public AudioSource death { get; private set; }
    }

    [Serializable]
    public struct Ambiance
    {
        [field:SerializeField] public AudioSource doorOpen { get; private set; }
        [field:SerializeField] public AudioSource doorClose { get; private set; }
        [field:SerializeField] public AudioSource gemCollect { get; private set; }
        [field:SerializeField] public AudioSource lever { get; private set; }
        [field:SerializeField] public AudioSource button { get; private set; }
        [field:SerializeField] public AudioSource wind { get; private set; }
        [field:SerializeField] public AudioSource platformMove { get; private set; }
    }
}
