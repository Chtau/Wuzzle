using Godot;
using Wuzzle.character.Interfaces;

public class Bullet : RigidBody2D
{
    private AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        animationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
    }

    private void OnTimerTimeout()
    {
        animationPlayer.Play("shutdown");
    }


    private void OnBulletBodyEntered(Object body)
    {
        if (body is IDamageReceiver receiver)
        {
            receiver.ReceiveHit(1f);
        }
    }
}