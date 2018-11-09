using Godot;
using System;

public class coin : Area2D
{
    private bool Taken = false;
    private AnimationPlayer AnimationPlayer;

    public override void _Ready()
    {
        AnimationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
        
    }

    public void OnCoinBodyEnter()
    {
        if (!Taken)
        {
            Taken = true;
            AnimationPlayer.Play("taken");
        }
    }
}