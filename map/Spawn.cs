using Godot;
using System;

public class Spawn : Area2D
{
    public enum Type
    {
        Spawn,
        Goal
    }

    public Type SpawnType { get; set; } = Type.Spawn;

    private CollisionShape2D collisionShape;

    public Vector2 SpawnGlobalPosition
    {
        get
        {
            return ((CollisionShape2D)GetNode("CollisionShape2D")).GlobalPosition;
            //return collisionShape.GlobalPosition;
        }
    }

    public override void _Ready()
    {
        collisionShape = (CollisionShape2D)GetNode("CollisionShape2D");
    }

    private void OnSpawnBodyEntered(Godot.Object body)
    {
        if (body is Player player)
        {
            player.SpawnEnteredCallback(SpawnType);
        }
    }
}
