using Godot;
using Wuzzle.character.Interfaces;

public class Bullet : RigidBody2D, IDamager
{
    private readonly Vector2 GravityVec = new Vector2(0, 900);
    private AnimationPlayer animationPlayer;
    private CollisionShape2D collision;
    private Sprite sprite;

    public float HitDamage => 5f;

    public override void _Ready()
    {
        animationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
        sprite = (Sprite)GetNode("Sprite");
        collision = (CollisionShape2D)GetNode("CollisionShape2D");
    }

    private void OnTimerTimeout()
    {
        animationPlayer.Play("shutdown");
    }

    private void OnBulletBodyEntered(Object body)
    {
        if (body is IDamageReceiver receiver)
        {
            receiver.ReceiveHit(HitDamage);
        } else if (body is Player)
        {
            GD.Print("Bullet hit player");
        }
    }
}