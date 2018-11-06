using Godot;
using System;

public class GoalItem : HBoxContainer
{
    public delegate void GoalHintDelegate(Wuzzle.Models.Goal goal);
    public event GoalHintDelegate GoalHint;
    public event EventHandler<Wuzzle.Models.Goal> GoalAnswer;

    private Wuzzle.Models.Goal goal;

    public void SetGoal(Wuzzle.Models.Goal goal)
    {
        this.goal = goal;

        var label = (Label)GetNode("Label");
        label.Text = this.goal.Title;
    }

    public override void _Ready()
    {
        
    }

    public void OnHintButtonPressed()
    {
        GoalHint?.Invoke(goal);
    }

    public void OnAnswerButtonPressed()
    {
        GoalAnswer?.Invoke(this, goal);
    }

}
