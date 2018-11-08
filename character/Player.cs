using Godot;
using System;

public class Player : KinematicBody2D
{
    Vector2 GravityVector = new Vector2(0, 900);
    Vector2 FloorNormal = new Vector2(0, -1);
    const float SlopeSlideStop = 25.0f;
    const float MinOnAirTime = 0.1f;
    const int WalkSpeed = 250; // pixels/sec
    const int JumpSpeed = 480;
    const int SidingChangeSpeed = 10;
    const int BulletVelocity = 1000;
    const float ShootTimeShowWeapon = 0.2f;

    Vector2 linear_vel = new Vector2();
    float onair_time = 0f;
    bool on_floor = false;
    float shoot_time = 99999; //time since last shot

    Sprite Sprite;

    //const float gravity = 200.0f;
    //const int walk_speed = 200;

    Vector2 velocity;


    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here
        Sprite = (Sprite)GetNode("Sprite");
    }

    public override void _PhysicsProcess(float delta)
    {
        onair_time += delta;
        shoot_time += delta;

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


        /*### ANIMATION ###

        var new_anim = "idle"


    if on_floor:
		if linear_vel.x < -SIDING_CHANGE_SPEED:
			sprite.scale.x = -1

            new_anim = "run"


        if linear_vel.x > SIDING_CHANGE_SPEED:
			sprite.scale.x = 1

            new_anim = "run"
	else:
		# We want the character to immediately change facing side when the player
		# tries to change direction, during air control.
		# This allows for example the player to shoot quickly left then right.
		if Input.is_action_pressed("move_left") and not Input.is_action_pressed("move_right"):
			sprite.scale.x = -1

        if Input.is_action_pressed("move_right") and not Input.is_action_pressed("move_left"):
			sprite.scale.x = 1


        if linear_vel.y < 0:
			new_anim = "jumping"
		else:
			new_anim = "falling"


    if shoot_time < SHOOT_TIME_SHOW_WEAPON:
		new_anim += "_weapon"


    if new_anim != anim:
		anim = new_anim
		$anim.play(anim)*/


        /*velocity.y += delta * gravity;

        if (Input.IsActionPressed("ui_left"))
        {
            velocity.x = -WalkSpeed;
        }
        else if (Input.IsActionPressed("ui_right"))
        {
            velocity.x = WalkSpeed;
        }
        else
        {
            velocity.x = 0;
        }

        // We don't need to multiply velocity by delta because MoveAndSlide already takes delta time into account.

        // The second parameter of MoveAndSlide is the normal pointing up.
        // In the case of a 2d platformer, in Godot upward is negative y, which translates to -1 as a normal.
        MoveAndSlide(velocity, new Vector2(0, -1));*/
    }
}
