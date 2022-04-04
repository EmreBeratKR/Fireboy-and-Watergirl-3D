public static class MyEventSystem
{
    public delegate void DefaultHandler();
    public delegate void GenericHandler<T>(T arg);
    public delegate void ParamsGenericHandler<T>(params T[] args);


    public static event GenericHandler<float> OnMusicVolumeSlided;
    public static event GenericHandler<float> OnSfxVolumeSlided;



    public static void RaiseMusicVolumeSlided(float value)
    {
        OnMusicVolumeSlided?.Invoke(value);
    }

    public static void RaiseSfxVolumeSlided(float value)
    {
        OnSfxVolumeSlided?.Invoke(value);
    }
}
