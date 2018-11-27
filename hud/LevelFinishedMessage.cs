using Godot;
using System;

public class LevelFinishedMessage : CanvasLayer
{
    private Label time;
    private Label previousRecord;
    private Label newRecord;
    private MarginContainer container;
    private Button nextLevel;

    public override void _Ready()
    {
        container = (MarginContainer)GetNode("MarginContainer");
        time = (Label)GetNode("MarginContainer/Panel/CenterContainer/VBoxContainer/TimeWrapper/Value");
        previousRecord = (Label)GetNode("MarginContainer/Panel/CenterContainer/VBoxContainer/PreviousRecordWrapper/Value");
        newRecord = (Label)GetNode("MarginContainer/Panel/CenterContainer/VBoxContainer/NewRecordWrapper/Label");
        nextLevel = (Button)GetNode("MarginContainer/Panel/CenterContainer/VBoxContainer/NextLevelWrapper/Button");

        newRecord.Visible = false;
        container.Visible = false;
    }

    private void OnButtonPressed()
    {
        // go to next level
        
    }

    public void Show(object levelValues)
    {
        container.Visible = true;
        nextLevel.Text = "Back to Menu"; // use levelmanager to check for next level and set correct text
        newRecord.Visible = true;
    }
}