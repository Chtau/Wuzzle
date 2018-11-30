using Godot;
using System;

public class StartMenu : Node
{
    enum ContentType
    {
        None,
        Levels,
        Options,
        Credits
    }

    private ScrollContainer options;
    private LevelSelect levels;
    private Panel contentPanel;

    public override void _Ready()
    {
        contentPanel = (Panel)GetNode("ContainerContent/Panel");
        levels = (LevelSelect)contentPanel.GetNode("MarginContainer/LevelSelect");
        options = (ScrollContainer)contentPanel.GetNode("MarginContainer/ScrollContainer");
        //optionsWrapper = (ScrollContainer)GetNode("MarginContainer/VBoxContainer/HBoxContainer/ScrollContainer");
        //levelSelect = (LevelSelect)GetNode("MarginContainer/VBoxContainer/HBoxContainer/LevelSelect");

        //optionsWrapper.Visible = false;
        //levelSelect.SetVisible(true);
        //levelSelect.Visible = true;

        OnShowContent(ContentType.None);
    }

    private void OnOptionsPressed()
    {
        OnShowContent(ContentType.Options);
    }

    private void OnCreditsPressed()
    {
        OnShowContent(ContentType.Credits);
    }

    private void OnLevelPressed()
    {
        OnShowContent(ContentType.Levels);
    }

    private void OnExitPressed()
    {
        // Replace with function body
        GetTree().Quit();
    }

    private void OnShowContent(ContentType contentType)
    {
        if (contentType == ContentType.Credits)
        {
            if (!contentPanel.IsVisible())
                contentPanel.Visible = true;
        } else if (contentType == ContentType.Options)
        {
            if (!contentPanel.IsVisible())
                contentPanel.Visible = true;
            levels.SetVisible(false);
            options.Visible = true;
        } else if (contentType == ContentType.Levels)
        {
            if (!contentPanel.IsVisible())
                contentPanel.Visible = true;
            levels.SetVisible(true);
            options.Visible = false;
        } else
        {
            contentPanel.Visible = false;
            levels.SetVisible(false);
            options.Visible = false;
        }
    }
}
