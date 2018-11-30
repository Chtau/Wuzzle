using Godot;
using System;

public class LevelMenu : Control
{
    private LevelItem levelItem;
    private Panel panel;

    public override void _Ready()
    {
        panel = (Panel)GetNode("CanvasLayer/Panel");
        this.Visible = false;
        panel.Visible = false;
    }

    private void OnClosePressed()
    {
        this.Visible = false;
        panel.Visible = false;
    }

    private void OnMenuPressed()
    {
        SharedFunctions.Instance.LoadScene(this, GlobalValues.MainMenuScene);
    }

    private void OnRestartPressed()
    {
        SharedFunctions.Instance.LoadScene(this, levelItem.ScenePath);
    }

    public void Show(LevelItem levelItem)
    {
        this.levelItem = levelItem;
        this.Visible = true;
        panel.Visible = true;
    }
}