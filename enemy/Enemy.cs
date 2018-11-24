using Godot;
using System;
using Wuzzle.character.Interfaces;

public class Enemy : KinematicBody2D, IDamageReceiver, IDamager
{
    [Export]
    public PackedScene Bullet { get; set; }

    enum State
    {
        Walking,
        Killed
    }

    private readonly Vector2 GravityVec = new Vector2(0, 900);
    private readonly Vector2 FloorNormal = new Vector2(0, -1);

    const float WalkSpeed = 70;

    private Vector2 linear_velocity = new Vector2();
    private float direction = -1;
    private string anim = "";

    private State state = State.Walking;

    private Sprite sprite;
    private RayCast2D detectFloorLeft;
    private RayCast2D detectWallLeft;
    private RayCast2D detectWallRight;
    private RayCast2D detectFloorRight;
    private AnimationPlayer animationPlayer;

    public Guid DashTargetId => Guid.NewGuid();

    public Node2D Instance => this;

    public Player.TargetTriggerType TargetTriggerType => Player.TargetTriggerType.None;

    public float HitDamage => state != State.Killed ? 1 : 0;

    private float shootTimeout = 0f;

    public override void _Ready()
    {
        sprite = (Sprite)GetNode("Sprite");
        detectFloorLeft = (RayCast2D)GetNode("DetectFloorLeft");
        detectWallLeft = (RayCast2D)GetNode("DetectWallLeft");
        detectWallRight = (RayCast2D)GetNode("DetectWallRight");
        detectFloorRight = (RayCast2D)GetNode("DetectFloorRight");
        animationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
    }

    public override void _PhysicsProcess(float delta)
    {
        string newAnim = "idle";
        shootTimeout += delta;

        if (state == State.Walking)
        {

            linear_velocity += GravityVec * delta;
            linear_velocity.x = direction * WalkSpeed;

            linear_velocity = MoveAndSlide(linear_velocity, FloorNormal);

            if (!detectFloorLeft.IsColliding() || detectWallLeft.IsColliding())
            {
                direction = 1.0f;
            }
            else if (!detectFloorRight.IsColliding() || detectWallRight.IsColliding())
                direction = -1.0f;

            sprite.Scale = new Vector2(direction, 1.0f);

            newAnim = "move";

            // shooting
            if (shootTimeout > .5)
            {
                var bullet = (Bullet)Bullet.Instance();
                bullet.Position = this.GlobalPosition;
                bullet.LinearVelocity = new Vector2(sprite.Scale.x * 1000, 0);
                bullet.AddCollisionExceptionWith(this);
                GetParent().AddChild(bullet);
                shootTimeout = 0;
            }
        }
        else if (state == State.Killed)
            newAnim = "explode";

        if (anim != newAnim)
        {
            anim = newAnim;
            animationPlayer.Play(anim);
        }
    }

    public void Hit()
    {
        state = State.Killed;
    }

    public void ReceiveHit(float damage)
    {
        Hit();
    }
}
