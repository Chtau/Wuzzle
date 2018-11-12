using Godot;
using System;

public class Box : StaticBody2D
{
    private bool Taken = false;
    private AnimationPlayer AnimationPlayer;
    private CollisionShape2D CollisionShape2D;

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
        CollisionShape2D.QueueFree();
        AnimationPlayer.Play("taken");
        return true;
    }
}
