using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Wuzzle.character;
using Wuzzle.character.Interfaces;

public class Player : KinematicBody2D
{
    Vector2 GravityVector = new Vector2(0, 900);
    Vector2 FloorNormal = new Vector2(0, -1);
    const float SlopeSlideStop = 25.0f;
    const float MinOnAirTime = 0.1f;
    const int WalkSpeed = 250; // pixels/sec
    const int JumpSpeed = 480;
    //const int MoveBurstSpeed = 250;
    const int SidingChangeSpeed = 10;

    Vector2 linear_vel = new Vector2();
    float onair_time = 0f;
    bool on_floor = false;
    string anim = "";

    Sprite Sprite;
    AnimationPlayer AnimationPlayer;
    Vector2 velocity;

    private Dash Dash;
    /*const float DashTimeout = 1f;
    float dash_time = 0f;
    private int MoveDashCount = 0;*/
    Area2D DashArea2D;

    public override void _Ready()
    {
        Sprite = (Sprite)GetNode("Sprite");
        AnimationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
        DashArea2D = (Area2D)GetNode("DashArea2D");
        Dash = new Dash(DashArea2D, this);
    }

    public override void _PhysicsProcess(float delta)
    {
        onair_time += delta;
        //dash_time += delta;

        // MOVEMENT
        // Apply Gravity
        linear_vel += delta * GravityVector;
        // Move and Slide
        linear_vel = MoveAndSlide(linear_vel, FloorNormal, SlopeSlideStop);
        // Detect Floor
        if (IsOnFloor())
            onair_time = 0;

        on_floor = onair_time < MinOnAirTime;

        Dash.ProcessPhysic(delta);
        /*foreach (var item in DashTargets)
        {
            CheckDashTargets(item.Value);
        }*/

        

        // CONTROL
        // Horizontal Movement
        var target_speed = 0;
        //bool moveLeft = false, moveRight = false, moveDown = false, moveUp = false;
        if (Input.IsActionPressed("move_left"))
        {
            target_speed += -1;
            //moveLeft = true;
        }
        if (Input.IsActionPressed("move_right"))
        {
            target_speed += 1;
            //moveRight = true;
        }
        /*if (Input.IsActionPressed("move_down"))
        {
            moveDown = true;
        }
        if (Input.IsActionPressed("move_up"))
        {
            moveUp = true;
        }*/

        // Dash
        /*if (target_speed != 0 && dash_time > DashTimeout && AllowDashMove && Input.IsActionJustPressed("move_dash"))
        {
            var target = OnGetDashTarget(moveLeft, moveRight, moveUp, moveDown);
            GD.Print(dash_time);
            target_speed *= MoveBurstSpeed;
            OnDashCountChange(-1);
            dash_time = 0;
        }*/

        target_speed *= WalkSpeed;

        linear_vel.x = Mathf.Lerp(linear_vel.x, target_speed, 0.1f);

        // Jumping
        if (on_floor && Input.IsActionJustPressed("jump"))
        {
            linear_vel.y = -JumpSpeed;
		    //$sound_jump.play()
        }

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
            //OnAddDashTarget(target);
        }
    }

    public void OnDashTargetBodyExited(object body)
    {
        if (body is IDashTarget target)
        {
            Dash.RemoveDashTarget(target);
            //OnRemoveDashTarget(target);
        }
    }

    /*public void AddDash()
    {
        OnDashCountChange(1);
    }*/

    /*private DashTargetItem OnGetDashTarget(bool left, bool right, bool up, bool down)
    {

        return null;
    }*/

    /*private void OnAddDashTarget(IDashTarget target)
    {
        DashTargets.Add(target.DashTargetId, new DashTargetItem
        {
            DashTarget = target,
            RayCast2D = GetDashRayCast2D(target)
        });
    }

    private void OnRemoveDashTarget(IDashTarget target)
    {
        if (DashTargets.ContainsKey(target.DashTargetId))
        {
            DashArea2D.RemoveChild(DashTargets[target.DashTargetId].RayCast2D);
            DashTargets[target.DashTargetId].RayCast2D.QueueFree();
            DashTargets.Remove(target.DashTargetId);
        }
        RemoveDebugLine(target.DashTargetId);
    }*/

    /*private RayCast2D GetDashRayCast2D(IDashTarget target)
    {
        var ray = new RayCast2D();
        ray.SetCollisionMaskBit(0, true);
        //ray.SetCollisionMaskBit(4, true);
        ray.AddException(this);
        ray.ExcludeParent = true;
        ray.Enabled = true;

        DashArea2D.AddChild(ray);
        return ray;
    }*/

    /*private void CheckDashTargets(DashTargetItem item)
    {
        item.RayCast2D.CastTo = item.DashTarget.Instance.GlobalPosition - this.GlobalPosition;
        if (item.RayCast2D.IsColliding())
        {
            var coll = item.RayCast2D.GetCollider();
            if (coll is IDashTarget targetCollider && item.DashTarget.DashTargetId == targetCollider.DashTargetId)
            {
                item.SetAngle(item.RayCast2D.Position.AngleToPoint(item.RayCast2D.CastTo));
                
                if (item.DashTargetDirection == DashTargetDirection.None)
                    GD.Print("Collide angle:" + item.Angle + " Dir:" + item.DashTargetDirection);
                //GD.Print("Collide angle:" + item.Angle + " Dir:" + item.DashTargetDirection + " Diag:" + item.DashTargetDiagonalDirection);
                AddDebugLine(item.DashTarget.DashTargetId, item.RayCast2D.Position, item.RayCast2D.CastTo);
            } else
            {
                RemoveDebugLine(item.DashTarget.DashTargetId);
            }
        }
    }*/

    /*System.Collections.Generic.Dictionary<Guid, Line2D> DebugLines = new System.Collections.Generic.Dictionary<Guid, Line2D>();
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
        if (DebugLines.ContainsKey(id))
        {
            this.RemoveChild(DebugLines[id]);
            DebugLines[id].QueueFree();
            DebugLines.Remove(id);
        }
    }*/

    /*private void OnDashCountChange(int changeValue)
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
    }*/
}
