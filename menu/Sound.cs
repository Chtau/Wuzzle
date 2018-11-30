using Godot;
using System;

public class Sound : VBoxContainer
{
    private HSlider music;
    private HSlider sfx;

    public override void _Ready()
    {
        SharedFunctions.Instance.AudioManager.BackgroundMusicDBChanged += AudioManager_BackgroundMusicDBChanged;
        SharedFunctions.Instance.AudioManager.SFXDBChanged += AudioManager_SFXDBChanged;

        music = (HSlider)GetNode("HBoxContainer/Music");
        sfx = (HSlider)GetNode("HBoxContainer2/SFX");

        GD.Print("Music: " + SharedFunctions.Instance.AudioManager.BackgroundMusicDB);
        music.Value = SharedFunctions.Instance.AudioManager.BackgroundMusicDB;
        sfx.Value = SharedFunctions.Instance.AudioManager.SFXDB;
    }

    private void AudioManager_SFXDBChanged(object sender, float e)
    {
        music.Value = SharedFunctions.Instance.AudioManager.BackgroundMusicDB;
    }

    private void AudioManager_BackgroundMusicDBChanged(object sender, float e)
    {
        sfx.Value = SharedFunctions.Instance.AudioManager.SFXDB;
    }

    private void OnMusicValueChanged(float value)
    {
        SharedFunctions.Instance.AudioManager.BackgroundMusicDB = value;
    }

    private void OnSFXValueChanged(float value)
    {
        SharedFunctions.Instance.AudioManager.SFXDB = value;
    }
}