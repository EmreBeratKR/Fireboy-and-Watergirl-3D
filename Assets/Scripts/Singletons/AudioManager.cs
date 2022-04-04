using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioContainer audios;


    public static float musicVolume { get => PlayerPrefs.GetFloat("MusicVol", 0.5f); set => PlayerPrefs.SetFloat("MusicVol", value); }
    public static float sfxVolume { get => PlayerPrefs.GetFloat("SFXVol", 0.75f); set => PlayerPrefs.SetFloat("SFXVol", value); }



    public static void StopAllAudios()
    {
        StopMusic();
        TryStopWind();
        TryStopDoorOpen();
        TryStopDoorClose();
        TryStopLiftMove();
    }

    public static void PlayMusic(LevelType type)
    {
        switch (type)
        {   
            case LevelType.Normal:
                AudioManager.Instance.audios.ost.normal.Play();
                return;
            case LevelType.Epic:
                AudioManager.Instance.audios.ost.epic.Play();
                return;
        }
    }

    public static void StopMusic()
    {
        AudioManager.Instance.audios.ost.normal.Stop();
        AudioManager.Instance.audios.ost.epic.Stop();
    }

    public static void PlayLevelFinish(LevelType type)
    {
        switch (type)
        {   
            case LevelType.Normal:
                AudioManager.Instance.audios.ost.normalFinish.Play();
                return;
            case LevelType.Epic:
                AudioManager.Instance.audios.ost.epicFinish.Play();
                return;
        }
    }

    public static void PlayGameover()
    {
        AudioManager.Instance.audios.ost.gameover.Play();
    }




    public static void PlayJump(Element element)
    {
        switch (element)
        {   
            case Element.Fire:
                AudioManager.Instance.audios.sfx.fireboy.jump.Play();
                return;
            case Element.Water:
                AudioManager.Instance.audios.sfx.watergirl.jump.Play();
                return;
        }
    }
    
    public static void PlayDeath(Element element)
    {
        switch (element)
        {   
            case Element.Fire:
                AudioManager.Instance.audios.sfx.fireboy.death.Play();
                return;
            case Element.Water:
                AudioManager.Instance.audios.sfx.watergirl.death.Play();
                return;
        }
    }

    public static void PlayGemCollect()
    {
        AudioManager.Instance.audios.sfx.ambiance.gemCollect.Play();
    }

    public static void PlayButtonToggle()
    {
        AudioManager.Instance.audios.sfx.ambiance.button.Play();
    }

    public static void PlayLeverPull()
    {
        AudioManager.Instance.audios.sfx.ambiance.lever.Play();
    }

    public static bool TryPlayLiftMove()
    {
        var audio = AudioManager.Instance.audios.sfx.ambiance.platformMove;

        if (audio.isPlaying) return false;

        audio.Play();
        return true;
    }

    public static bool TryStopLiftMove()
    {
        var audio = AudioManager.Instance.audios.sfx.ambiance.platformMove;

        if (!audio.isPlaying) return false;

        audio.Stop();
        return true;
    }

    public static bool TryPlayWind()
    {
        var audio = AudioManager.Instance.audios.sfx.ambiance.wind;

        if (audio.isPlaying) return false;
        
        audio.Play();
        return true;
    }

    public static bool TryStopWind()
    {
        var audio = AudioManager.Instance.audios.sfx.ambiance.wind;

        if (!audio.isPlaying) return false;
        
        audio.Stop();
        return true;
    }

    public static bool TryPlayDoorOpen()
    {
        var audio = AudioManager.Instance.audios.sfx.ambiance.doorOpen;

        if (audio.isPlaying) return false;

        audio.Play();
        return true;
    }

    public static bool TryStopDoorOpen()
    {
        var audio = AudioManager.Instance.audios.sfx.ambiance.doorOpen;

        if (!audio.isPlaying) return false;

        audio.Stop();
        return true;
    }

    public static bool TryPlayDoorClose()
    {
        var audio = AudioManager.Instance.audios.sfx.ambiance.doorClose;

        if (audio.isPlaying) return false;

        audio.Play();
        return true;
    }

    public static bool TryStopDoorClose()
    {
        var audio = AudioManager.Instance.audios.sfx.ambiance.doorClose;

        if (!audio.isPlaying) return false;

        audio.Stop();
        return true;
    }


    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
        MyEventSystem.OnMusicVolumeSlided += UpdateMusicVolume;
        MyEventSystem.OnSfxVolumeSlided += UpdateSfxVolume;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
        MyEventSystem.OnMusicVolumeSlided -= UpdateMusicVolume;
        MyEventSystem.OnSfxVolumeSlided -= UpdateSfxVolume;
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        StopAllAudios();
    }

    private void UpdateMusicVolume(float value)
    {
        audios.ost.normal.volume = value;
        audios.ost.epic.volume = value;

        audios.ost.normalFinish.volume = value;
        audios.ost.epicFinish.volume = value;
    }

    private void UpdateSfxVolume(float value)
    {
        audios.ost.gameover.volume = value;

        audios.sfx.fireboy.jump.volume = value;
        audios.sfx.fireboy.death.volume = value;
        audios.sfx.watergirl.jump.volume = value;
        audios.sfx.watergirl.death.volume = value;

        audios.sfx.ambiance.doorOpen.volume = value;
        audios.sfx.ambiance.doorClose.volume = value;
        audios.sfx.ambiance.gemCollect.volume = value;
        audios.sfx.ambiance.lever.volume = value;
        audios.sfx.ambiance.button.volume = value;
        audios.sfx.ambiance.wind.volume = value;
        audios.sfx.ambiance.platformMove.volume = value;
    }
}
