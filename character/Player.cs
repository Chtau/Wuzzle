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
        Strike,
        GotHit,
    }

    public enum TargetTriggerType
    {
        None,
        Question
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
    float strikeTime = 0f;
    float gotHitTime = 0f;

    CollisionShape2D CollisionShape2D;
    Question question;

    public override void _Ready()
    {
        characterSprite = (Sprite)GetNode("Sprite2");
        characterAnimationPlayer = (AnimationPlayer)characterSprite.GetNode("AnimationPlayer");
        CollisionShape2D = (CollisionShape2D)GetNode("CollisionShape2D");
        question = (Question)GetNode("../Question");
    }

    //Vector2 vect = new Vector2();
    private float gotHitSpeed = 0f;
    private float dashTimeout = 0f;

    public override void _PhysicsProcess(float delta)
    {
        onair_time += delta;
        if (State == PlayerPhysicsState.Strike)
            strikeTime += delta;

        if (State == PlayerPhysicsState.Dash)
        {
            if (dashTimeout > .15)
            {
                dashTimeout = 0f;
                linear_vel.x = 0;
                State = PlayerPhysicsState.Idle;
            }
            else
            {
                dashTimeout += delta;
            }
        }

        if (State == PlayerPhysicsState.GotHit)
        {
            if (gotHitTime > .5)
            {
                State = PlayerPhysicsState.Idle;
                gotHitTime = 0f;
                linear_vel.x = 0;
                gotHitSpeed = 0f;
            }
            else
            {
                if (gotHitTime == 0)
                {
                    if (Input.IsActionPressed(GlobalValues.Keymap_Move_Left))
                    {
                        gotHitSpeed += 1;
                    }
                    if (Input.IsActionPressed(GlobalValues.Keymap_Move_Right))
                    {
                        gotHitSpeed += -1;
                    }
                }
                gotHitTime += delta;

                

                linear_vel.x = Mathf.Lerp(linear_vel.x, gotHitSpeed * WalkSpeed, 0.1f);
                linear_vel += delta * GravityVector;

                linear_vel = MoveAndSlide(linear_vel, FloorNormal, SlopeSlideStop);

                return;
            }
        }

        // MOVEMENT
        // Apply Gravity
        // Move and Slide
        if (State != PlayerPhysicsState.Dash)
        {
            linear_vel += delta * GravityVector;
            linear_vel = MoveAndSlide(linear_vel, FloorNormal, SlopeSlideStop);
        } else
        {
            linear_vel += delta * GravityVector;
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

        if (State != PlayerPhysicsState.Dash) // && State != PlayerPhysicsState.Strike
        {
            if (Input.IsActionJustPressed(GlobalValues.Keymap_Move_Dash))
            {

                bool moveLeft = false, moveRight = false;

                if (Input.IsActionPressed(GlobalValues.Keymap_Move_Left))
                {
                    moveLeft = true;
                }
                if (Input.IsActionPressed(GlobalValues.Keymap_Move_Right))
                {
                    moveRight = true;
                }

                if (moveLeft || moveRight)
                {
                    var target_speed_x = 0;
                    if (moveRight)
                        target_speed_x += 1;
                    else if (moveLeft)
                        target_speed_x += -1;
                    target_speed_x *= 25000;

                    linear_vel.x = Mathf.Lerp(linear_vel.x, target_speed_x, 0.1f);

                    State = PlayerPhysicsState.Dash;
                }
            } else
            {
                // CONTROL
                // Horizontal Movement
                if (Input.IsActionPressed(GlobalValues.Keymap_Move_Left))
                {
                    target_speed += -1;
                }
                if (Input.IsActionPressed(GlobalValues.Keymap_Move_Right))
                {
                    target_speed += 1;
                }

                target_speed *= WalkSpeed;
                linear_vel.x = Mathf.Lerp(linear_vel.x, target_speed, 0.1f);

                // Jumping
                if (on_floor && Input.IsActionJustPressed(GlobalValues.Keymap_Move_Jump))
                {
                    linear_vel.y = -JumpSpeed;
                    //$sound_jump.play()
                }

                if (Input.IsActionJustPressed(GlobalValues.Keymap_Move_Strike))
                {
                    State = PlayerPhysicsState.Strike;
                    //GD.Print("execute Strike");
                }
            }

        }

        // ANIMATION
        string new_anim = "idle";

        if (on_floor)
        {
            if (linear_vel.x < -SidingChangeSpeed)
            {
                characterSprite.Scale = new Vector2(-0.25f, characterSprite.Scale.y);
                new_anim = "move";
            } else if (linear_vel.x > SidingChangeSpeed)
            {
                characterSprite.Scale = new Vector2(.25f, characterSprite.Scale.y);
                new_anim = "move";
            }
            
        } else
        {
            if (Input.IsActionPressed(GlobalValues.Keymap_Move_Left) && !Input.IsActionPressed(GlobalValues.Keymap_Move_Right))
                characterSprite.Scale = new Vector2(-.25f, characterSprite.Scale.y);
            if (Input.IsActionPressed(GlobalValues.Keymap_Move_Right) && !Input.IsActionPressed(GlobalValues.Keymap_Move_Left))
                characterSprite.Scale = new Vector2(.25f, characterSprite.Scale.y);
            if (linear_vel.y < 0)
                new_anim = "jump";
            else
                new_anim = "fall";
        }
        if (Input.IsActionJustPressed(GlobalValues.Keymap_Move_Strike))
            new_anim = "strike";

        //GD.Print("State: " + Enum.GetName(typeof(PlayerPhysicsState), State));

        if (new_anim != anim)
        {
            if (State == PlayerPhysicsState.Strike)
            {
                if (strikeTime > .5)
                {
                    //GD.Print("Striketime: " + strikeTime);
                    State = PlayerPhysicsState.Idle;
                    strikeTime = 0f;
                } else
                {
                    //GD.Print("Striketime: " + strikeTime);
                    if (anim != "strike")
                    {
                        anim = "strike";
                        characterAnimationPlayer.Play(anim, -1, 5);
                    }
                }
            }
            else
            {
                anim = new_anim;
                characterAnimationPlayer.Play(anim);
            }
        }
    }

    public void OnMeleeBodyEnter(object body)
    {
        if (body is IDamager damager)
        {
            if (damager.HitDamage > 0)
            {
                GD.Print("Player got hit! Dmg:" + damager.HitDamage);
                State = PlayerPhysicsState.GotHit;
            }
        } else
        {
            //GD.Print("Melee Body Enter: " + body);
        }
    }

    public void OnDashTargetReachBodyEnter(object body)
    {
        if (body is IPickup pickup)
        {
            if (pickup.Interact())
            {
                if (pickup.TargetTrigger == TargetTriggerType.Question)
                    question.ShowQuestion();
            }
        }
    }
}
