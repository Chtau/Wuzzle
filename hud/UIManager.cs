using Godot;
using System;

public class UIManager : Node
{
    private LevelStartMessage levelStartMessage;
    private LevelFinishedMessage levelFinishedMessage;
    private LevelGameOverMessage levelGameOverMessage;
    private LevelMenu levelMenu;
    private Question question;

    public LevelItem LevelItem { get; set; }

    public override void _Ready()
    {
        question = (Question)GetNode("Question");
        levelStartMessage = (LevelStartMessage)GetNode("LevelStartMessage");
        levelFinishedMessage = (LevelFinishedMessage)GetNode("LevelFinishedMessage");
        levelGameOverMessage = (LevelGameOverMessage)GetNode("LevelGameOverMessage");
        levelMenu = (LevelMenu)GetNode("LevelMenu");

        levelFinishedMessage.Visible = false;
        levelGameOverMessage.Visible = false;
        levelStartMessage.Visible = false;
        question.Visible = false;
        levelMenu.Visible = false;
    }

    public void ShowMenu()
    {
        levelMenu.Show(LevelItem);
        levelMenu.SetFocus();
    }

    public bool IsOpenMenu()
    {
        return levelMenu.IsOpen();
    }

    public void CloseMenu()
    {
        levelMenu.Clear();
    }

    public void ShowQuestion()
    {
        question.AddShowQuestion();
    }

    public void ShowStartMessage(Action callback)
    {
        levelStartMessage.Show(() =>
        {
            callback.Invoke();
        });
    }

    public void ShowFinishedMessage(TimeSpan gameTime)
    {
        levelMenu.Clear();
        question.ClearQuestions();
        levelFinishedMessage.Show(LevelItem, gameTime);
        levelFinishedMessage.SetFocus();
    }

    public void ShowGameOverMessage()
    {
        levelMenu.Clear();
        question.ClearQuestions();
        levelGameOverMessage.Show(LevelItem);
        levelGameOverMessage.SetFocus();
    }
}
