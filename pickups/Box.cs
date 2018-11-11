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
        AnimationPlayer.Play("taken");
        return true;
    }

    public void OnBoxBodyEnter(object body)
    {
        if (!Taken)
        {
            if (body is Player player)
            {
                //player.AddBurst();
                Taken = true;
                AnimationPlayer.Play("taken");
            }
        }
    }
}
