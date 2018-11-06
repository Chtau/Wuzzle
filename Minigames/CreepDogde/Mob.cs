using Godot;
using System;

public class Mob : RigidBody2D
{
    [Export]
    public int MinSpeed = 150; // Minimum speed range.

    [Export]
    public int MaxSpeed = 250; // Maximum speed range.

    private String[] _mobTypes = { "walk", "swim", "fly" };

    public override void _Ready()
    {
        var animatedSprite = (AnimatedSprite)GetNode("AnimatedSprite");

        // C# doesn't implement GDScript's random methods, so we use 'Random'
        // as an alternative.
        //
        // Note: Never define random multiple times in real projects. Create a
        // class memory and reuse it to get true random numbers.
        var randomMob = new Random();
        animatedSprite.Animation = _mobTypes[randomMob.Next(0, _mobTypes.Length)];
    }

    public void onVisibilityScreenExited()
    {
        QueueFree();
    }
}
