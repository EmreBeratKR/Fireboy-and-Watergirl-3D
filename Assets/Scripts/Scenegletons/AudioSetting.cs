using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSetting : Scenegleton<AudioSetting>
{
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;


    private void Start()
    {
        musicVolumeSlider.value = AudioManager.musicVolume;
        sfxVolumeSlider.value = AudioManager.sfxVolume;
        OnMusicVolumeSlided();
        OnSfxVolumeSlided();
    }

    public void OnMusicVolumeSlided()
    {
        AudioManager.musicVolume = musicVolumeSlider.value;
        MyEventSystem.RaiseMusicVolumeSlided(musicVolumeSlider.value);
    }

    public void OnSfxVolumeSlided()
    {
        AudioManager.sfxVolume = sfxVolumeSlider.value;
        MyEventSystem.RaiseSfxVolumeSlided(sfxVolumeSlider.value);
    }
}
