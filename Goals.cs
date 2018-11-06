using Godot;
using System;

public class Goals : CanvasLayer
{
    [Export]
    public PackedScene GoalItem;

    public override void _Ready()
    {
        var container = (VBoxContainer)GetNode("GoalListContainer");

        foreach (var item in GoalManager.Instance.CurrentGoalList())
        {
            var goalItem = (GoalItem)GoalItem.Instance();
            goalItem.SetGoal(item);
            goalItem.GoalHint += OnGoalItemHint;
            goalItem.GoalAnswer += GoalItem_GoalAnswer;
            container.AddChild(goalItem);
        }
    }

    private void GoalItem_GoalAnswer(object sender, Wuzzle.Models.Goal e)
    {
        var dialog = (AnswerDialog)GetNode("AnswerDialog");
        dialog.SetGoal(e);
        dialog.ShowModal(true);
    }

    public void OnGoalItemHint(Wuzzle.Models.Goal goal)
    {
        var hint = (RichTextLabel)GetNode("HintRTX");
        string content = "";
        foreach (var item in GoalManager.Instance.GoalHints(goal.Id))
        {
            content += item.Text;
        }
        hint.Text = content;
    }
}
