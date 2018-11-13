using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Player : KinematicBody2D
{
    Vector2 GravityVector = new Vector2(0, 900);
    Vector2 FloorNormal = new Vector2(0, -1);
    const float SlopeSlideStop = 25.0f;
    const float MinOnAirTime = 0.1f;
    const int WalkSpeed = 250; // pixels/sec
    const int JumpSpeed = 480;
    const int MoveBurstSpeed = 250;
    const int SidingChangeSpeed = 10;
    const int BulletVelocity = 1000;
    const float ShootTimeShowWeapon = 0.2f;

    Vector2 linear_vel = new Vector2();
    float onair_time = 0f;
    bool on_floor = false;
    float shoot_time = 99999; //time since last shot
    string anim = "";

    Sprite Sprite;
    AnimationPlayer AnimationPlayer;
    Vector2 velocity;

    const float DashTimeout = 1f;
    float dash_time = 0f;
    private int MoveDashCount = 0;
    RayCast2D DashRayCast2D;

    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here
        Sprite = (Sprite)GetNode("Sprite");
        AnimationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
        DashRayCast2D = (RayCast2D)GetNode("DashArea2D/DashRayCast2D");
        DashRayCast2D.AddException(this);
    }

    public override void _PhysicsProcess(float delta)
    {
        onair_time += delta;
        shoot_time += delta;
        dash_time += delta;

        // MOVEMENT
        // Apply Gravity
        linear_vel += delta * GravityVector;
        // Move and Slide
        linear_vel = MoveAndSlide(linear_vel, FloorNormal, SlopeSlideStop);
	    // Detect Floor
	    if (IsOnFloor())
            onair_time = 0;

        on_floor = onair_time < MinOnAirTime;

        // CONTROL
        // Horizontal Movement
        var target_speed = 0;

        if (Input.IsActionPressed("move_left"))
            target_speed += -1;

        if (Input.IsActionPressed("move_right"))
            target_speed += 1;

        if (target_speed != 0 && dash_time > DashTimeout && AllowDashMove && Input.IsActionJustPressed("move_dash"))
        {
            GD.Print(dash_time);
            target_speed *= MoveBurstSpeed;
            OnDashCountChange(-1);
            dash_time = 0;
        }

        target_speed *= WalkSpeed;

        linear_vel.x = Mathf.Lerp(linear_vel.x, target_speed, 0.1f);

        // Jumping
        if (on_floor && Input.IsActionJustPressed("jump"))
        {
            linear_vel.y = -JumpSpeed;
		    //$sound_jump.play()
        }

        // Shooting
        /*if Input.is_action_just_pressed("shoot"):
		var bullet = preload("res://bullet.tscn").instance()

        bullet.position = $sprite / bullet_shoot.global_position #use node for shoot position
		bullet.linear_velocity = Vector2(sprite.scale.x * BULLET_VELOCITY, 0)

        bullet.add_collision_exception_with(self) # don't want player to collide with bullet
		get_parent().add_child(bullet) #don't want bullet to move with me, so add it as child of parent
		$sound_shoot.play()

        shoot_time = 0*/


        // ANIMATION

        string new_anim = "idle";

        if (on_floor)
        {
            if (linear_vel.x < -SidingChangeSpeed)
            {
                Sprite.Scale = new Vector2(-1, Sprite.Scale.y);
                new_anim = "run";
            } else if (linear_vel.x > SidingChangeSpeed)
            {
                Sprite.Scale = new Vector2(1, Sprite.Scale.y);
                new_anim = "run";
            }
            
        } else
        {
            if (Input.IsActionPressed("move_left") && !Input.IsActionPressed("move_right"))
                Sprite.Scale = new Vector2(-1, Sprite.Scale.y);
            if (Input.IsActionPressed("move_right") && !Input.IsActionPressed("move_left"))
                Sprite.Scale = new Vector2(1, Sprite.Scale.y);
            if (linear_vel.y < 0)
                new_anim = "jumping";
            else
                new_anim = "falling";
        }

        if (new_anim != anim)
        {
            anim = new_anim;
            AnimationPlayer.Play(anim);
        }

        foreach (var item in DashTargets)
        {
            CheckDashTargets(item.Value);
        }
    }

    public void OnBodyEnter(object body)
    {
        if (body is Box box)
        {
            if (box.PlayerInteract())
            {
                AddDash();
            }
        }
    }

    System.Collections.Generic.Dictionary<Guid, Box> DashTargets = new System.Collections.Generic.Dictionary<Guid, Box>();
    public void OnDashTargetBodyEnter(object body)
    {
        if (body is Box box)
        {
            //CheckDashTargets(box);
            DashTargets.Add(box.DashTargetId, box);
        }
    }

    public void OnDashTargetBodyExited(object body)
    {
        if (body is Box box)
        {
            //CheckDashTargets(box);
            DashTargets.Remove(box.DashTargetId);
            RemoveDebugLine(box.DashTargetId);
        }
    }

    public void AddDash()
    {
        OnDashCountChange(1);
    }

    private void CheckDashTargets(Box box)
    {
        DashRayCast2D.CastTo = box.GlobalPosition - this.GlobalPosition;
        DashRayCast2D.Enabled = true;

        if (DashRayCast2D.IsColliding())
        {
            var coll = DashRayCast2D.GetCollider();
            if (coll is Box boxCollider && box.DashTargetId == boxCollider.DashTargetId)
            {
                //GD.Print("Can collide with Box");
                AddDebugLine(box.DashTargetId, DashRayCast2D.Position, DashRayCast2D.CastTo);
            }
        }
    }

    System.Collections.Generic.Dictionary<Guid, Line2D> DebugLines = new System.Collections.Generic.Dictionary<Guid, Line2D>();
    private void AddDebugLine(Guid id, Vector2 pos, Vector2 tar)
    {
        if (DebugLines.ContainsKey(id))
        {
            for (int i = 0; i < DebugLines[id].Points.Length; i++)
            {
                DebugLines[id].RemovePoint(i);
            }
            DebugLines[id].AddPoint(pos);
            DebugLines[id].AddPoint(tar);
        }
        else
        {
            var line = new Line2D
            {
                DefaultColor = new Color(255, 0, 119.53f, 255),
                Width = 10
            };
            line.AddPoint(pos);
            line.AddPoint(tar);
            this.AddChild(line);
            DebugLines.Add(id, line);
        }
    }

    private void RemoveDebugLine(Guid id)
    {
        this.RemoveChild(DebugLines[id]);
        DebugLines.Remove(id);
    }

    private void OnDashCountChange(int changeValue)
    {
        MoveDashCount += changeValue;
        GD.Print("Current Burst:" + MoveDashCount);
    }

    private bool AllowDashMove
    {
        get
        {
            if (MoveDashCount > 0)
                return true;
            return false;
        }
    }
}