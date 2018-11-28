using Godot;
using System;

public class LevelGameOverMessage : CanvasLayer
{
    private Panel panel;
    private LevelItem levelItem;

    public override void _Ready()
    {
        panel = (Panel)GetNode("Panel");

        panel.Visible = false;
    }

    private void OnTryAgainPressed()
    {
        SharedFunctions.Instance.LoadScene(this, levelItem.ScenePath);
    }

    private void OnBackPressed()
    {
        SharedFunctions.Instance.LoadScene(this, GlobalValues.MainMenuScene);
    }

    public void Show(LevelItem levelItem)
    {
        this.levelItem = levelItem;
        panel.Visible = true;
    }
}