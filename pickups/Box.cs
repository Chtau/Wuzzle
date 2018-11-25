using Godot;
using System;

public class Box : StaticBody2D, IPickup
{
    private bool Taken = false;
    private AnimationPlayer AnimationPlayer;
    private CollisionShape2D CollisionShape2D;

    public Node2D Instance { get => this; }

    public Player.TargetTriggerType TargetTrigger => Player.TargetTriggerType.Question;

    public Box()
    {
    }

    public override void _Ready()
    {
        CollisionShape2D = (CollisionShape2D)GetNode("CollisionShape2D");
        AnimationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
        AnimationPlayer.Play("idle");
    }

    public bool Interact()
    {
        if (Taken)
            return false;
        Taken = true;
        CollisionShape2D.Disabled = true;
        //CollisionShape2D.QueueFree();
        AnimationPlayer.Play("taken");
        return true;
    }
}
