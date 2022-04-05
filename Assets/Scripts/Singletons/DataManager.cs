using System;
using System.IO;
using UnityEngine;

public class DataManager : Scenegleton<DataManager>
{
    private const string levelStatusBaseFileName = "level_status_";

    private string rootPath => Application.persistentDataPath;
    private string levelStatusesDirectory => rootPath + "/Level Statuses/";

    
    public static void SaveLevelStatus(LevelData levelData)
    {
        string directory = DataManager.Instance.levelStatusesDirectory;

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (levelData.levelStatus > 0)
        {
            LevelStatus oldStatus = LoadLevelStatus(levelData.levelNumber);
            
            if (levelData.levelStatus < oldStatus) return;
        }

        string jsonData = JsonUtility.ToJson(levelData);

        string path = directory + levelStatusBaseFileName + levelData.levelNumber + ".json";

        File.WriteAllText(path, jsonData);
    }

    public static LevelStatus LoadLevelStatus(int levelNumber)
    {
        string directory = DataManager.Instance.levelStatusesDirectory;

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string path = directory + levelStatusBaseFileName + levelNumber + ".json";

        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);

            LevelData loadedData = JsonUtility.FromJson<LevelData>(jsonData);

            return loadedData.levelStatus;
        }

        LevelData defaultData = LevelData.Default;
        defaultData.levelNumber = levelNumber;

        SaveLevelStatus(defaultData);

        return defaultData.levelStatus;
    }
}

[Serializable]
public struct LevelData
{
    public int levelNumber;
    public LevelStatus levelStatus;

    public static LevelData Default => new LevelData();

    public LevelData(int levelNumber, LevelStatus levelStatus)
    {
        this.levelNumber = levelNumber;
        this.levelStatus = levelStatus;
    }
}
