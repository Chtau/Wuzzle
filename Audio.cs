using Godot;
using System;

public class Audio : Node
{
    const string PlayerHit = "res://assests/audio/skweak1.wav";
    const string PlayerDash = "res://assests/audio/sfx_throw.wav";
    const string PlayerStrike = "res://assests/audio/knifesharpener1.wav";
    const string PlayerJump = "res://assests/audio/sfx_jump.wav";

    public enum SFX
    {
        Hit,
        Dash,
        Strike,
        Jump
    }

    private AudioStreamPlayer sfx;
    private AudioStreamPlayer backgroundMusic;
    private bool playBackground;
    private AudioStream playerHitResource;
    private AudioStream playerDashResource;
    private AudioStream playerStrikeResource;
    private AudioStream playerJumpResource;

    public override void _Ready()
    {
        backgroundMusic = (AudioStreamPlayer)GetNode("BackgroundMusic");
        sfx = (AudioStreamPlayer)GetNode("SFX");

        playerHitResource = (AudioStream)GD.Load(PlayerHit);
        playerDashResource = (AudioStream)GD.Load(PlayerDash);
        playerStrikeResource = (AudioStream)GD.Load(PlayerStrike);
        playerJumpResource = (AudioStream)GD.Load(PlayerJump);

        SharedFunctions.Instance.AudioManager.BackgroundMusicDBChanged += AudioManager_BackgroundMusicChanged;
        SharedFunctions.Instance.AudioManager.SFXDBChanged += AudioManager_SFXDBChanged;

        if (playBackground)
            OnPlayBackground();
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

    public void PlaySFX(SFX sFX)
    {
        if (!sfx.IsPlaying())
        {
            if (sFX == SFX.Hit)
                sfx.Stream = playerHitResource;
            else if (sFX == SFX.Dash)
                sfx.Stream = playerDashResource;
            else if (sFX == SFX.Strike)
                sfx.Stream = playerStrikeResource;
            else if (sFX == SFX.Jump)
                sfx.Stream = playerJumpResource;

            sfx.VolumeDb = SharedFunctions.Instance.AudioManager.SFXDB;
            sfx.Play();
        }
    }
}
