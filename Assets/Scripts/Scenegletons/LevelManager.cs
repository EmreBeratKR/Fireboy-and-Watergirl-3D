using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Scenegleton<LevelManager>
{
    public const int levelStart = 5;
    public const int levelEnd = 15;
    private const char Seperator = '-';

    [field:SerializeField] public LevelType type { get; private set; }


    public static int levelCount => levelEnd - levelStart + 1;


    public static int ToSceneIndex(int levelNumber)
    {
        return levelStart + levelNumber - 1;
    }

    public static int ToLevelNumber(int sceneIndex)
    {
        return sceneIndex - levelStart + 1;
    }


    private void Start()
    {
        AudioManager.StopMusic();
        AudioManager.PlayMusic(this.type);
    }

    private void OnEnable()
    {
        MyEventSystem.OnLevelCompleted += SaveLevelStatuses;
    }

    private void OnDisable()
    {
        MyEventSystem.OnLevelCompleted -= SaveLevelStatuses;
    }

    private void SaveLevelStatuses(int levelNumber, LevelStatus levelStatus)
    {
        DataManager.SaveLevelStatus(new LevelData(levelNumber, levelStatus));
    }

}

public enum LevelType { Normal, Epic }

public enum LevelStatus { NotFinished, SemiFinished, FullFinished }
