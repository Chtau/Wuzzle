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
    private AnimationPlayer animationSprite;
    private Sprite shootPoint;
    private CollisionShape2D collision;
    private AudioStreamPlayer sfx;

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
        shootPoint = (Sprite)GetNode("ShootPoint");
        animationSprite = (AnimationPlayer)sprite.GetNode("AnimationPlayer");
        collision = (CollisionShape2D)GetNode("CollisionShape2D");
        sfx = (AudioStreamPlayer)GetNode("SFX");

        sfx.VolumeDb = SharedFunctions.Instance.AudioManager.SFXDB;
        SharedFunctions.Instance.AudioManager.SFXDBChanged += AudioManager_SFXDBChanged;
    }

    public override void _ExitTree()
    {
        SharedFunctions.Instance.AudioManager.SFXDBChanged -= AudioManager_SFXDBChanged;
        base._ExitTree();
    }

    private void AudioManager_SFXDBChanged(object sender, float e)
    {
        if (sfx != null)
            sfx.VolumeDb = e;
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

            if (detectWallLeft.IsColliding())
            {
                direction = 1.0f;
            } else if (detectWallRight.IsColliding())
            {
                direction = -1.0f;
            }
            if (!detectFloorLeft.IsColliding())
            {
                direction = 1.0f;
            } else if (!detectFloorRight.IsColliding())
            {
                direction = -1.0f;
            }

            if (direction < 0)
                sprite.Scale = new Vector2(.35f, .35f);
            else
                sprite.Scale = new Vector2(-.35f, .35f);

            newAnim = "walk";

            // shooting
            if (shootTimeout > 2)
            {
                var bullet = (Bullet)Bullet.Instance();
                bullet.Position = shootPoint.GlobalPosition;
                if (direction < 0)
                    bullet.LinearVelocity = new Vector2(-1f * 200, -100);
                else
                    bullet.LinearVelocity = new Vector2(1f * 200, -100);
                bullet.AddCollisionExceptionWith(this);
                GetParent().AddChild(bullet);
                shootTimeout = 0;
            }
        }
        else if (state == State.Killed)
            newAnim = "killed";

        if (anim != newAnim)
        {
            anim = newAnim;
            animationSprite.Play(anim);
        }
    }

    public void Hit()
    {
        state = State.Killed;
        collision.Disabled = true;
        sfx.Play();
        SharedFunctions.Instance.AudioManager.SFXDBChanged -= AudioManager_SFXDBChanged;
    }

    public void ReceiveHit(float damage)
    {
        Hit();
    }
}
