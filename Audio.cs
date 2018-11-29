using Godot;
using System;

public class Audio : Node
{
    private AudioStreamPlayer backgroundMusic;
    private bool playBackground;

    public override void _Ready()
    {
        backgroundMusic = (AudioStreamPlayer)GetNode("BackgroundMusic");
        if (playBackground)
            backgroundMusic.Play(SharedFunctions.Instance.AudioManager.BackgroundMusicPosition);
    }

    public void PlayBackground()
    {
        playBackground = true;
        if (backgroundMusic != null)
        {
            backgroundMusic.Play(SharedFunctions.Instance.AudioManager.BackgroundMusicPosition);
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
}
