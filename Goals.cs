using Godot;
using System;

public class Goals : CanvasLayer
{

    public override void _Ready()
    {
        var container = (VBoxContainer)GetNode("GoalListContainer");

        foreach (var item in GoalManager.Instance.CurrentGoalList())
        {
            var checkBox = new CheckBox()
            {
                ToggleMode = false,
                //Disabled = true,
                Pressed = item.Item2,
                Text = item.Item3,
                Name = "CheckBox" + item.Item1
            };
            //button_down
            checkBox.Connect("button_down", this, nameof(OnGoalCheckboxPressed), new Godot.Array { item.Item1.ToString(), checkBox });
            container.AddChild(checkBox);
        }
    }

    public void OnGoalCheckboxPressed(string goalId, CheckBox checkBox)
    {
        //var check = (CheckBox)GetNode("CheckBox" + goalId);
        checkBox.Text = "Pressed";
    }
}
