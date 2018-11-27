using Godot;
using System;

public class LevelSelectItem : Control
{
    public LevelItem LevelItem { get; private set; }

    private Label title;
    private Label desciption;
    private Label record;

    public override void _Ready()
    {
        title = (Label)GetNode("MarginContainer/Panel/HBoxContainer/Title");
        desciption = (Label)GetNode("MarginContainer/Panel/HBoxContainer/VBoxContainer/Description");
        record = (Label)GetNode("MarginContainer/Panel/HBoxContainer/VBoxContainer/Record");
        OnLevelItemChanged();
    }

    private void OnButtonPressed()
    {
        SharedFunctions.Instance.LoadScene(this, LevelItem.ScenePath);
    }

    public void ChangeLevelItem(LevelItem levelItem)
    {
        LevelItem = levelItem;
        OnLevelItemChanged();
    }

    private void OnLevelItemChanged()
    {
        if (LevelItem != null && title != null && desciption != null && record != null)
        {
            title.Text = LevelItem.LevelTitle;
            desciption.Text = LevelItem.LevelDescription;
            if (LevelItem.Record.HasValue)
                record.Text = SharedFunctions.Instance.FormatTimeSpan(LevelItem.Record.Value);
            else
                record.Text = "--:--:--";
        }
    }
}