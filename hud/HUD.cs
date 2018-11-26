using Godot;
using System;

public class HUD : CanvasLayer
{

    private Label lifeNumber;
    private TextureProgress lifebar;
    private Label timeText;

    public override void _Ready()
    {
        lifeNumber = (Label)GetNode("Wrapper/HBoxContainer/Bars/Bar/Count/Background/HBoxContainer/LifeNumber");
        lifebar = (TextureProgress)GetNode("Wrapper/HBoxContainer/Bars/Bar/Lifebar");
        timeText = (Label)GetNode("Wrapper/HBoxContainer/Counters/Count2/Background/HBoxContainer/TimeText");

        SharedFunctions.Instance.GameState.CurrentLifeChanged += GameState_CurrentLifeChanged;
        SharedFunctions.Instance.GameState.LevelTimeChanged += GameState_LevelTimeChanged;
        SharedFunctions.Instance.GameState.MaxLifeChanged += GameState_MaxLifeChanged;
    }

    public override void _ExitTree()
    {
        SharedFunctions.Instance.GameState.CurrentLifeChanged -= GameState_CurrentLifeChanged;
        SharedFunctions.Instance.GameState.LevelTimeChanged -= GameState_LevelTimeChanged;
        SharedFunctions.Instance.GameState.MaxLifeChanged -= GameState_MaxLifeChanged;
        base._ExitTree();
    }

    private void GameState_MaxLifeChanged(object sender, float e)
    {
        lifebar.MaxValue = e;
    }

    private void GameState_LevelTimeChanged(object sender, TimeSpan e)
    {
        OnTimeChange(e);
    }

    private void GameState_CurrentLifeChanged(object sender, float e)
    {
        OnLifeChange(e);
    }

    public void Init(float maxLife, float currentLife, TimeSpan startTime)
    {
        lifebar.MaxValue = maxLife;
        lifebar.Value = currentLife;
        timeText.Text = OnFormatTimeSpan(startTime);
    }

    private void OnLifeChange(float newValue)
    {
        lifebar.Value = newValue;
    }

    private void OnTimeChange(TimeSpan timeSpan)
    {
        timeText.Text = OnFormatTimeSpan(timeSpan);
    }

    private string OnFormatTimeSpan(TimeSpan timeSpan)
    {
        return timeSpan.ToString("mm':'ss':'fff");
    }
}
