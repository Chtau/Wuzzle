using Godot;
using System;

public class AnswerDialog : WindowDialog
{
    private Wuzzle.Models.Goal Goal;
    private RichTextLabel ResultRTXT;
    private Label QuestionLbl;
    private LineEdit AnswerInput;

    public override void _Ready()
    {
        QuestionLbl = (Label)GetNode("VBoxContainer/GoalContainer/GoalQuestion");
        ResultRTXT = (RichTextLabel)GetNode("VBoxContainer/ResultContainer/ResultRTXT");
        AnswerInput = (LineEdit)GetNode("VBoxContainer/AnswerContainer/AnswerInput");
    }

    public void OnCheckAnswerPressed()
    {
        var result = GoalManager.Instance.VerifyAnswer(Goal, AnswerInput.Text);
        ResultRTXT.Text = result.Result;
    }

    public void OnCancelPressed()
    {
        this.Hide();
    }

    public void SetGoal(Wuzzle.Models.Goal goal)
    {
        Goal = goal;
        QuestionLbl.Text = Goal.Title;
        ResultRTXT.Text = "";
        AnswerInput.Text = "";
    }

}