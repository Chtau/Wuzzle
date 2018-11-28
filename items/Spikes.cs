using Godot;
using System;
using Wuzzle.character.Interfaces;

public class Spikes : Node2D, IDamager
{
    public float HitDamage => 50f;

    public override void _Ready()
    {
        
    }
}