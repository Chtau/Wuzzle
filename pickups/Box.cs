using Godot;
using System;

public class Box : StaticBody2D, Wuzzle.character.Interfaces.IDashTarget
{
    private bool Taken = false;
    private AnimationPlayer AnimationPlayer;
    private CollisionShape2D CollisionShape2D;

    public Guid DashTargetId { get; }
    public Node2D Instance { get => this; }

    public Player.TargetTriggerType TargetTriggerType => Player.TargetTriggerType.Question;

    public Box()
    {
        DashTargetId = Guid.NewGuid();
    }

    public override void _Ready()
    {
        CollisionShape2D = (CollisionShape2D)GetNode("CollisionShape2D");
        AnimationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
        AnimationPlayer.Play("idle");
    }

    public bool PlayerInteract()
    {
        if (Taken)
            return false;
        Taken = true;
        CollisionShape2D.Disabled = true;
        //CollisionShape2D.QueueFree();
        AnimationPlayer.Play("taken");
        return true;
    }

    public void BeforeDashTo()
    {
        CollisionShape2D.Disabled = true;
    }

    public bool Reached()
    {
        return Taken;
    }
}
