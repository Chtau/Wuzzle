using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Wuzzle.character;
using Wuzzle.character.Interfaces;

public class Player : KinematicBody2D
{
    public enum PlayerPhysicsState
    {
        Idle,
        Move,
        Dash,
        Jump,
        Fall,
    }

    private Sprite characterSprite;
    private AnimationPlayer characterAnimationPlayer;

    private PlayerPhysicsState State = PlayerPhysicsState.Idle;

    Vector2 GravityVector = new Vector2(0, 900);
    Vector2 FloorNormal = new Vector2(0, -1);
    const float SlopeSlideStop = 25.0f;
    const float MinOnAirTime = 0.1f;
    const int WalkSpeed = 250; // pixels/sec
    const int JumpSpeed = 480;
    const int SidingChangeSpeed = 10;

    Vector2 linear_vel = new Vector2();
    float onair_time = 0f;
    bool on_floor = false;
    string anim = "";

    //Sprite Sprite;
    //AnimationPlayer AnimationPlayer;
    Vector2 velocity;
    CollisionShape2D CollisionShape2D;

    private Dash Dash;
    Area2D DashArea2D;

    public override void _Ready()
    {
        characterSprite = (Sprite)GetNode("Sprite2");
        characterAnimationPlayer = (AnimationPlayer)characterSprite.GetNode("AnimationPlayer");
        CollisionShape2D = (CollisionShape2D)GetNode("CollisionShape2D");
        //Sprite = (Sprite)GetNode("Sprite");
        //AnimationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
        DashArea2D = (Area2D)GetNode("DashArea2D");
        Dash = new Dash(DashArea2D, this, CollisionShape2D);

        //var charNode = Character.Instance();
        //AddChild(charNode);
        //characterSprite = (Sprite)charNode.GetChild(0);
        //characterSprite.Centered = true;

        //characterAnimationPlayer = (AnimationPlayer)characterSprite.FindNode("AnimationPlayer");
        
    }

    Vector2 vect = new Vector2();

    public override void _PhysicsProcess(float delta)
    {
        onair_time += delta;

        State = Dash.ProcessPhysic(State, delta, ref linear_vel);

        // MOVEMENT
        // Apply Gravity
        // Move and Slide
        if (State != PlayerPhysicsState.Dash)
        {
            linear_vel += delta * GravityVector;
            linear_vel = MoveAndSlide(linear_vel, FloorNormal, SlopeSlideStop);
        } else
        {
            linear_vel = MoveAndSlide(linear_vel, FloorNormal, SlopeSlideStop);
        }
        
        // Detect Floor
        if (IsOnFloor())
        {
            if (State != PlayerPhysicsState.Dash)
                onair_time = 0;
            else
            {
                GD.Print("Hit Floor");
                linear_vel.x = 0;
                linear_vel.y = 0;
                State = PlayerPhysicsState.Idle;
            }
        }

        if (IsOnWall())
        {
            if (State == PlayerPhysicsState.Dash)
            {
                GD.Print("Hit Wall");
                linear_vel.x = 0;
                linear_vel.y = 0;
                State = PlayerPhysicsState.Idle;
            }
        }

        on_floor = onair_time < MinOnAirTime;

        var target_speed = 0;

        if (State != PlayerPhysicsState.Dash)
        {
            // CONTROL
            // Horizontal Movement
            if (Input.IsActionPressed("move_left"))
            {
                target_speed += -1;
            }
            if (Input.IsActionPressed("move_right"))
            {
                target_speed += 1;
            }

            target_speed *= WalkSpeed;
            linear_vel.x = Mathf.Lerp(linear_vel.x, target_speed, 0.1f);

            // Jumping
            if (on_floor && Input.IsActionJustPressed("jump"))
            {
                linear_vel.y = -JumpSpeed;
                //$sound_jump.play()
            }
        }

        // ANIMATION
        string new_anim = "idle";

        if (on_floor)
        {
            if (linear_vel.x < -SidingChangeSpeed)
            {
                characterSprite.Scale = new Vector2(-0.25f, characterSprite.Scale.y);
                //Sprite.Scale = new Vector2(-1, Sprite.Scale.y);
                new_anim = "move";// "run";
            } else if (linear_vel.x > SidingChangeSpeed)
            {
                characterSprite.Scale = new Vector2(.25f, characterSprite.Scale.y);
                //Sprite.Scale = new Vector2(1, Sprite.Scale.y);
                new_anim = "move";// "run";
            }
            
        } else
        {
            if (Input.IsActionPressed("move_left") && !Input.IsActionPressed("move_right"))
                characterSprite.Scale = new Vector2(-.25f, characterSprite.Scale.y);
            //Sprite.Scale = new Vector2(-1, Sprite.Scale.y);
            if (Input.IsActionPressed("move_right") && !Input.IsActionPressed("move_left"))
                characterSprite.Scale = new Vector2(.25f, characterSprite.Scale.y);
            //Sprite.Scale = new Vector2(1, Sprite.Scale.y);
            if (linear_vel.y < 0)
                new_anim = "jump";
            //new_anim = "jumping";
            else
                new_anim = "fall";
        }

        if (new_anim != anim)
        {
            anim = new_anim;
            characterAnimationPlayer.Play(anim);
        }
    }

    public void OnMeleeBodyEnter(object body)
    {
        if (body is Box box)
        {
            if (box.PlayerInteract())
            {
                Dash.AddDash();
            }
        }
    }

    public void OnDashTargetBodyEnter(object body)
    {
        if (body is IDashTarget target)
        {
            Dash.AddDashTarget(target);
        }
    }

    public void OnDashTargetBodyExited(object body)
    {
        if (body is IDashTarget target)
        {
            Dash.RemoveDashTarget(target);
        }
    }
}