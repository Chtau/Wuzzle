using Godot;
using System;

public class Box : StaticBody2D
{
    private bool Taken = false;
    private AnimationPlayer AnimationPlayer;

    public override void _Ready()
    {
        AnimationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
        AnimationPlayer.Play("idle");
    }

    public bool PlayerInteract()
    {
        if (Taken)
            return false;
        Taken = true;
        ((CollisionShape2D)GetNode("CollisionShape2D")).QueueFree();
        AnimationPlayer.Play("taken");
        return true;
    }
}
