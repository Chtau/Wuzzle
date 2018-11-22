using Godot;
using System;
using Wuzzle.character.Interfaces;

public class Sword : Sprite, IDamager
{
    public float HitDamage => 1f;

    public override void _Ready()
    {
        
    }

    public void OnBodyEntered(Godot.Object body)
    {
        if (IsVisible())
        {
            if (body is IDamageReceiver damageReceiver)
            {
                damageReceiver.ReceiveHit(HitDamage);
            }
        }
    }
}