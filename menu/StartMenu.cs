using Godot;
using System;

public class StartMenu : Node
{
    private ScrollContainer optionsWrapper;
    private LevelSelect levelSelect;

    public override void _Ready()
    {
        optionsWrapper = (ScrollContainer)GetNode("MarginContainer/VBoxContainer/HBoxContainer/ScrollContainer");
        levelSelect = (LevelSelect)GetNode("MarginContainer/VBoxContainer/HBoxContainer/LevelSelect");

        optionsWrapper.Visible = false;
        levelSelect.SetVisible(true);
        //levelSelect.Visible = true;
    }

    private void _on_FadeIn_FadeInFinished(object foo, object bar)
    {
        // Replace with function body
    }

    private void OnOptionsPressed()
    {
        levelSelect.SetVisible(false);
        optionsWrapper.Visible = true;
    }

    private void OnStartPressed()
    {
        // Replace with function body
        optionsWrapper.Visible = false;
        levelSelect.SetVisible(true);
    }

    private void OnExitPressed()
    {
        // Replace with function body
        GetTree().Quit();
    }
}