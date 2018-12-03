using Godot;
using System;

public class LevelSelectItem : Control
{
    public LevelItem LevelItem { get; private set; }

    private Label title;
    private Label desciption;
    private Label record;
    private TextureRect medailleGold;
    private TextureRect medailleSilber;
    private TextureRect medailleBronze;
    private Button button;

    public override void _Ready()
    {
        title = (Label)GetNode("Panel/HBoxContainer/Title");
        desciption = (Label)GetNode("Panel/HBoxContainer/VBoxContainer/Description");
        record = (Label)GetNode("Panel/HBoxContainer/VBoxContainer/RecordWrapper/Record");
        medailleGold = (TextureRect)GetNode("Panel/HBoxContainer/VBoxContainer/RecordWrapper/MedailleGold");
        medailleSilber = (TextureRect)GetNode("Panel/HBoxContainer/VBoxContainer/RecordWrapper/MedailleSilber");
        medailleBronze = (TextureRect)GetNode("Panel/HBoxContainer/VBoxContainer/RecordWrapper/MedailleBronze");
        button = (Button)GetNode("Panel/HBoxContainer/Button");

        medailleSilber.Visible = false;
        medailleGold.Visible = false;
        medailleBronze.Visible = false;

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
            {
                if (LevelItem.Record.Value < LevelItem.GoldTime)
                {
                    medailleGold.Visible = true;
                } else if (LevelItem.Record.Value < LevelItem.SilverTime)
                {
                    medailleSilber.Visible = true;
                }
                else if (LevelItem.Record.Value < LevelItem.BronzeTime)
                {
                    medailleBronze.Visible = true;
                }
                record.Text = SharedFunctions.Instance.FormatTimeSpan(LevelItem.Record.Value);
            }
            else
                record.Text = "--:--:--";
        }
    }

    public void SetFocus()
    {
        button.GrabFocus();
    }
}