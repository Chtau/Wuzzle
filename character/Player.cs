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
        Waiting,
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

    public float MaxLife { get; private set; } = 50;
    public float CurrentLife { get; private set; } = 50;
    public TimeSpan LevelGameTime { get; private set; } = new TimeSpan();
    public DateTime LevelStartTime { get; private set; } = DateTime.UtcNow;

    private Sprite characterSprite;
    private AnimationPlayer characterAnimationPlayer;

    private PlayerPhysicsState State = PlayerPhysicsState.Idle;

    Vector2 GravityVector = new Vector2(0, 900);
    Vector2 FloorNormal = new Vector2(0, -1);
    const float SlopeSlideStop = 25.0f;
    const float MinOnAirTime = 0.1f;
    const int WalkSpeed = 350; // pixels/sec
    const int JumpSpeed = 480;
    const int SidingChangeSpeed = 10;

    Vector2 linear_vel = new Vector2();
    float onair_time = 0f;
    bool on_floor = false;
    string anim = "";
    float strikeTime = 0f;
    float gotHitTime = 0f;

    private CollisionShape2D CollisionShape2D;

    private float dashTimeout = 0f;

    private IID levelConfiguration;
    private Spawn spawn;
    private Spawn goal;
    private Area2D gotHitArea;
    private CollisionShape2D gotHitCollision;
    private Audio audio;

    private LevelItem levelItem;

    private UIManager uiManager;

    public override void _Ready()
    {
        SharedFunctions.Instance.Init();
        SharedFunctions.Instance.GameState.LevelAnsweredQuestionsChanged += GameState_LevelAnsweredQuestionsChanged;
        SharedFunctions.Instance.GameState.LevelAnsweredQuestionsWrongChanged += GameState_LevelAnsweredQuestionsWrongChanged;

        characterSprite = (Sprite)GetNode("Sprite2");
        characterAnimationPlayer = (AnimationPlayer)characterSprite.GetNode("AnimationPlayer");
        characterAnimationPlayer.Connect("animation_finished", this, nameof(OnAnimationFinished));
        CollisionShape2D = (CollisionShape2D)GetNode("CollisionShape2D");
        levelConfiguration = (IID)GetParent().GetParent();
        levelItem = SharedFunctions.Instance.LevelManager.ById(levelConfiguration.Id);
        spawn = (Spawn)GetNode("../Spawn");
        spawn.SpawnType = Spawn.Type.Spawn;
        goal = (Spawn)GetNode("../Goal");
        goal.SpawnType = Spawn.Type.Goal;
        goal.Deactivated();
        gotHitArea = (Area2D)GetNode("Area2D");
        gotHitCollision = (CollisionShape2D)GetNode("Area2D/CollisionShape2D");
        audio = (Audio)GetNode("../Audio");
        uiManager = (UIManager)GetNode("../UIManager");
        uiManager.LevelItem = levelItem;
        
        if (GetTree().IsPaused())
            GetTree().Paused = false;

        OnLevelLoad();
    }

    public override void _ExitTree()
    {
        audio.PauseBackground();
        SharedFunctions.Instance.GameState.LevelAnsweredQuestionsWrongChanged -= GameState_LevelAnsweredQuestionsWrongChanged;
        SharedFunctions.Instance.GameState.LevelAnsweredQuestionsChanged -= GameState_LevelAnsweredQuestionsChanged;
        base._ExitTree();
    }


    private void GameState_LevelAnsweredQuestionsWrongChanged(object sender, int e)
    {
        int maxFalseQuestions = (levelItem.TotalQuestionAvailable - levelItem.RequieredQuestions);
        if (maxFalseQuestions < SharedFunctions.Instance.GameState.LevelAnsweredQuestionsWrong)
        {
            OnLevelGameOver();
        }
    }

    private void GameState_LevelAnsweredQuestionsChanged(object sender, int e)
    {
        if (SharedFunctions.Instance.GameState.LevelAnsweredQuestions >= SharedFunctions.Instance.GameState.LevelRequieredQuestions)
        {
            goal.Active();
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (State == PlayerPhysicsState.Waiting)
        {

            // allow the gravity to do it's work
            linear_vel += delta * GravityVector;
            linear_vel = MoveAndSlide(linear_vel, FloorNormal, SlopeSlideStop);
        }
        else
        {
            if (Input.IsActionJustPressed("ui_cancel"))
            {
                uiManager.ShowMenu();
                //levelMenu.Show(levelItem);
            }

            LevelGameTime = DateTime.UtcNow - LevelStartTime;
            SharedFunctions.Instance.GameState.LevelTime = LevelGameTime;

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
                }
            }

            // MOVEMENT
            // Apply Gravity
            // Move and Slide
            if (State != PlayerPhysicsState.Dash)
            {
                linear_vel += delta * GravityVector;
                linear_vel = MoveAndSlide(linear_vel, FloorNormal, SlopeSlideStop);
            }
            else
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
                        audio.PlaySFX(Audio.SFX.Dash);
                    }
                }
                else
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
                        audio.PlaySFX(Audio.SFX.Jump);
                    }

                    if (Input.IsActionJustPressed(GlobalValues.Keymap_Move_Strike))
                    {
                        State = PlayerPhysicsState.Strike;
                        audio.PlaySFX(Audio.SFX.Strike);
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
                }
                else if (linear_vel.x > SidingChangeSpeed)
                {
                    characterSprite.Scale = new Vector2(.25f, characterSprite.Scale.y);
                    new_anim = "move";
                }

            }
            else
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
            if (Input.IsActionJustPressed(GlobalValues.Keymap_Move_Dash))
                new_anim = "dash";
            if (Input.IsActionJustPressed(GlobalValues.Keymap_Move_Strike))
                new_anim = "strike";

            if (allowAnimation && new_anim != anim)
            {
                if (State == PlayerPhysicsState.Strike)
                {
                    if (strikeTime > .5)
                    {
                        State = PlayerPhysicsState.Idle;
                        strikeTime = 0f;
                    }
                    else
                    {
                        if (anim != "strike")
                        {
                            anim = "strike";
                            allowAnimation = false;
                            characterAnimationPlayer.Play(anim, -1, 5);
                        }
                    }
                } else if (State == PlayerPhysicsState.Dash)
                {
                    if (anim != "dash")
                    {
                        anim = "dash";
                        allowAnimation = false;
                        characterAnimationPlayer.Play(anim, -1, 5);
                    }
                }
                else
                {
                    anim = new_anim;
                    characterAnimationPlayer.Play(anim);
                }
            }
        }
    }

    public void OnMeleeBodyEnter(object body)
    {
        if (State != PlayerPhysicsState.Waiting)
        {
            if (body is IDamager damager)
            {
                if (damager.HitDamage > 0)
                {
                    CurrentLife += -1 * damager.HitDamage;
                    SharedFunctions.Instance.GameState.CurrentLife = CurrentLife;
                    State = PlayerPhysicsState.GotHit;
                    audio.PlaySFX(Audio.SFX.Hit);
                    if (CurrentLife <= 0)
                    {
                        CurrentLife = 0;
                        SharedFunctions.Instance.GameState.CurrentLife = CurrentLife;
                        OnLevelGameOver();
                    }
                    damager.AfterHit();
                }
            }
        }
    }

    public void OnDashTargetReachBodyEnter(object body)
    {
        if (body is IPickup pickup)
        {
            if (pickup.Interact())
            {
                if (pickup.TargetTrigger == TargetTriggerType.Question)
                    uiManager.ShowQuestion();
            }
        }
    }

    private void OnLevelLoad()
    {
        State = PlayerPhysicsState.Waiting;

        OnSetCharacterPosition();

        MaxLife = 50;
        SharedFunctions.Instance.GameState.MaxLife = MaxLife;
        CurrentLife = 50;
        SharedFunctions.Instance.GameState.CurrentLife = CurrentLife;
        SharedFunctions.Instance.GameState.LevelAnsweredQuestions = 0;
        SharedFunctions.Instance.GameState.LevelAnsweredQuestionsWrong = 0;
        SharedFunctions.Instance.GameState.LevelRequieredQuestions = levelItem.RequieredQuestions;

        spawn.Active();

        uiManager.ShowStartMessage(() =>
        {
            LevelGameTime = new TimeSpan();
            LevelStartTime = DateTime.UtcNow;

            State = PlayerPhysicsState.Idle;
            spawn.Deactivated();
            audio.PlayBackground();
        });
    }

    private void OnSetCharacterPosition()
    {
        this.GlobalPosition = spawn.SpawnGlobalPosition;
    }

    public void SpawnEnteredCallback(Spawn.Type type)
    {
        if (type == Spawn.Type.Goal && State != PlayerPhysicsState.Waiting
            && SharedFunctions.Instance.GameState.LevelAnsweredQuestions >= SharedFunctions.Instance.GameState.LevelRequieredQuestions)
        {
            // level is finished
            goal.Deactivated();
            OnLevelFinished();
        }
    }

    private void OnLevelFinished()
    {
        State = PlayerPhysicsState.Waiting;
        uiManager.ShowFinishedMessage(LevelGameTime);
        GetTree().Paused = true;
    }

    private void OnLevelGameOver()
    {
        State = PlayerPhysicsState.Waiting;
        uiManager.ShowGameOverMessage();
        GetTree().Paused = true;
    }

    private bool allowAnimation = true;
    private void OnAnimationFinished(string animation)
    {
        if (animation == "strike")
        {
            State = PlayerPhysicsState.Idle;
            strikeTime = 0f;
            allowAnimation = true;
        } else if (animation == "dash")
        {
            State = PlayerPhysicsState.Idle;
            allowAnimation = true;
        }
    }
}
