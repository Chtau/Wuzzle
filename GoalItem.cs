using Godot;
using System;

public class GoalItem : HBoxContainer
{
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
        EmitSignal("GoalHint", goal);
    }

}
