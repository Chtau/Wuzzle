using Godot;
using System;

public class Box : Area2D
{
    private bool Taken = false;
    private AnimationPlayer AnimationPlayer;

    public override void _Ready()
    {
        AnimationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
        AnimationPlayer.Play("idle");
    }

    public void OnBoxBodyEnter(object body)
    {
        if (!Taken)
        {
            if (body is Player player)
            {
                player.AddBurst();
                Taken = true;
                AnimationPlayer.Play("taken");
            }
        }
    }
}
