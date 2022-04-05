public static class MyEventSystem
{
    public delegate void DefaultHandler();
    public delegate void GenericHandler<T>(T arg);
    public delegate void DoubleGenericHandler<T, T1>(T arg, T1 arg1);
    public delegate void ParamsGenericHandler<T>(params T[] args);


    public static event GenericHandler<float> OnMusicVolumeSlided;
    public static event GenericHandler<float> OnSfxVolumeSlided;

    public static event GenericHandler<bool> OnLevelNumberToggled;

    public static event DoubleGenericHandler<int, LevelStatus> OnLevelCompleted;


    public static void RaiseMusicVolumeSlided(float value)
    {
        OnMusicVolumeSlided?.Invoke(value);
    }

    public static void RaiseSfxVolumeSlided(float value)
    {
        OnSfxVolumeSlided?.Invoke(value);
    }


    public static void RaiseLevelNumberToggled(bool isOn)
    {
        OnLevelNumberToggled?.Invoke(isOn);
    }


    public static void RaiseLevelCompleted(int levelNumber, LevelStatus levelStatus)
    {
        OnLevelCompleted?.Invoke(levelNumber, levelStatus);
    }
}
