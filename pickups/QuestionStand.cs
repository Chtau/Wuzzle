using Godot;
using System;

public class QuestionStand : StaticBody2D, IPickup
{
    private bool Taken = false;
    private AnimationPlayer animation;
    private CollisionPolygon2D collision;
    private AudioStreamPlayer sfx;

    public Player.TargetTriggerType TargetTrigger => Player.TargetTriggerType.Question;

    public override void _Ready()
    {
        sfx = (AudioStreamPlayer)GetNode("SFX");
        collision = (CollisionPolygon2D)GetNode("CollisionPolygon2D");
        animation = (AnimationPlayer)GetNode("AnimationPlayer");
        animation.Play("idle");

        sfx.VolumeDb = SharedFunctions.Instance.AudioManager.SFXDB;
        SharedFunctions.Instance.AudioManager.SFXDBChanged += AudioManager_SFXDBChanged;
    }

    public override void _ExitTree()
    {
        SharedFunctions.Instance.AudioManager.SFXDBChanged -= AudioManager_SFXDBChanged;
        base._ExitTree();
    }

    private void AudioManager_SFXDBChanged(object sender, float e)
    {
        if (sfx != null)
            sfx.VolumeDb = e;
    }

    public bool Interact()
    {
        if (Taken)
            return false;
        Taken = true;
        collision.Disabled = true;
        animation.Play("taken");
        sfx.Play();
        //sfx.Stop();
        SharedFunctions.Instance.AudioManager.SFXDBChanged -= AudioManager_SFXDBChanged;
        return true;
    }
}