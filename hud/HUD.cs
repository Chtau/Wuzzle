using Godot;
using System;

public class HUD : CanvasLayer
{

    private Label lifeNumber;
    private TextureProgress lifebar;
    private Label timeText;
    private Label questionCountText;

    public override void _Ready()
    {
        lifeNumber = (Label)GetNode("Wrapper/HBoxContainer/Bars/Bar/Count/Background/HBoxContainer/LifeNumber");
        lifebar = (TextureProgress)GetNode("Wrapper/HBoxContainer/Bars/Bar/Lifebar");
        timeText = (Label)GetNode("Wrapper/HBoxContainer/Counters/Count2/Background/HBoxContainer/TimeText");
        questionCountText = (Label)GetNode("MarginContainer/HBoxContainer/NinePatchRect/HBoxContainer2/QuestionCountText");

        SharedFunctions.Instance.GameState.CurrentLifeChanged += GameState_CurrentLifeChanged;
        SharedFunctions.Instance.GameState.LevelTimeChanged += GameState_LevelTimeChanged;
        SharedFunctions.Instance.GameState.MaxLifeChanged += GameState_MaxLifeChanged;
        SharedFunctions.Instance.GameState.LevelAnsweredQuestionsChanged += GameState_LevelAnsweredQuestionsChanged;
        SharedFunctions.Instance.GameState.LevelRequieredQuestionsChanged += GameState_LevelRequieredQuestionsChanged;
    }

    public override void _ExitTree()
    {
        SharedFunctions.Instance.GameState.CurrentLifeChanged -= GameState_CurrentLifeChanged;
        SharedFunctions.Instance.GameState.LevelTimeChanged -= GameState_LevelTimeChanged;
        SharedFunctions.Instance.GameState.MaxLifeChanged -= GameState_MaxLifeChanged;
        SharedFunctions.Instance.GameState.LevelAnsweredQuestionsChanged += GameState_LevelAnsweredQuestionsChanged;
        SharedFunctions.Instance.GameState.LevelRequieredQuestionsChanged += GameState_LevelRequieredQuestionsChanged;
        base._ExitTree();
    }

    private void GameState_LevelRequieredQuestionsChanged(object sender, int e)
    {
        OnQuestionCountChange(SharedFunctions.Instance.GameState.LevelAnsweredQuestions, e);
    }

    private void GameState_LevelAnsweredQuestionsChanged(object sender, int e)
    {
        OnQuestionCountChange(e, SharedFunctions.Instance.GameState.LevelRequieredQuestions);
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
        timeText.Text = SharedFunctions.Instance.FormatTimeSpan(startTime);
    }

    private void OnLifeChange(float newValue)
    {
        lifebar.Value = newValue;
        lifeNumber.Text = newValue.ToString();
    }

    private void OnTimeChange(TimeSpan timeSpan)
    {
        timeText.Text = SharedFunctions.Instance.FormatTimeSpan(timeSpan);
    }

    private void OnQuestionCountChange(int questionAnswered, int questionTotal)
    {
        questionCountText.Text = questionAnswered + "/" + questionTotal;
    }
}
