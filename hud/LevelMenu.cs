using Godot;
using System;

public class LevelMenu : Control
{
    private LevelItem levelItem;
    private NinePatchRect panel;
    private Button closeButton;

    public override void _Ready()
    {
        panel = (NinePatchRect)GetNode("CanvasLayer/Panel");
        closeButton = (Button)panel.GetNode("MarginContainer/VBoxContainer/HBoxContainer/Close");
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

    public void Clear()
    {
        this.OnClosePressed();
    }

    public bool IsOpen()
    {
        if (Visible && panel.Visible)
            return true;
        return false;
    }

    public void SetFocus()
    {
        closeButton.GrabFocus();
    }
}