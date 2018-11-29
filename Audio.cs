using Godot;
using System;

public class Audio : Node
{
    private AudioStreamPlayer sfx;
    private AudioStreamPlayer backgroundMusic;
    private bool playBackground;

    public override void _Ready()
    {
        backgroundMusic = (AudioStreamPlayer)GetNode("BackgroundMusic");
        sfx = (AudioStreamPlayer)GetNode("SFX");

        SharedFunctions.Instance.AudioManager.BackgroundMusicDBChanged += AudioManager_BackgroundMusicChanged;
        SharedFunctions.Instance.AudioManager.SFXDBChanged += AudioManager_SFXDBChanged;

        if (playBackground)
            OnPlayBackground();

        //GD.Load()
    }

    private void AudioManager_SFXDBChanged(object sender, float e)
    {
        if (sfx != null)
        {
            sfx.VolumeDb = e;
        }
    }

    public override void _ExitTree()
    {
        SharedFunctions.Instance.AudioManager.BackgroundMusicDBChanged -= AudioManager_BackgroundMusicChanged;
        SharedFunctions.Instance.AudioManager.SFXDBChanged -= AudioManager_SFXDBChanged;
        base._ExitTree();
    }

    private void AudioManager_BackgroundMusicChanged(object sender, float e)
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.VolumeDb = e;
        }
    }

    public void PlayBackground()
    {
        playBackground = true;
        if (backgroundMusic != null)
        {
            OnPlayBackground();
        }
    }

    public void PauseBackground()
    {
        SharedFunctions.Instance.AudioManager.BackgroundMusicPosition = backgroundMusic.GetPlaybackPosition();
        backgroundMusic.Stop();
    }

    public void StopBackground()
    {
        backgroundMusic.Stop();
        SharedFunctions.Instance.AudioManager.BackgroundMusicPosition = 0f;
    }

    private void OnPlayBackground()
    {
        backgroundMusic.VolumeDb = SharedFunctions.Instance.AudioManager.BackgroundMusicDB;
        backgroundMusic.Play(SharedFunctions.Instance.AudioManager.BackgroundMusicPosition);
    }
}
