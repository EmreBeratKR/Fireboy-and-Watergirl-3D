using UnityEngine;

public class LevelManager : Scenegleton<LevelManager>
{
    [field:SerializeField] public LevelType type { get; private set; }


    private void Start()
    {
        AudioManager.StopMusic();
        AudioManager.PlayMusic(this.type);   
    }
}

public enum LevelType { Normal, Epic }
