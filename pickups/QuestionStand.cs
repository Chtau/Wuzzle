using Godot;
using System;

public class QuestionStand : StaticBody2D, IPickup
{
    private bool Taken = false;
    private AnimationPlayer animation;
    private CollisionPolygon2D collision;

    public Player.TargetTriggerType TargetTrigger => Player.TargetTriggerType.Question;

    public override void _Ready()
    {
        collision = (CollisionPolygon2D)GetNode("CollisionPolygon2D");
        animation = (AnimationPlayer)GetNode("AnimationPlayer");
        animation.Play("idle");
    }

    public bool Interact()
    {
        if (Taken)
            return false;
        Taken = true;
        collision.Disabled = true;
        animation.Play("taken");
        return true;
    }
}