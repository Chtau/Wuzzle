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

    private Control options;
    private LevelSelect levels;
    private Panel contentPanel;
    private Control credits;
    private Button levelSelect;

    public override void _Ready()
    {
        SharedFunctions.Instance.Init();

        contentPanel = (Panel)GetNode("ContainerContent/Panel");
        levels = (LevelSelect)contentPanel.GetNode("MarginContainer/LevelSelect");
        options = (Control)contentPanel.GetNode("MarginContainer/Options");
        credits = (Control)contentPanel.GetNode("MarginContainer/Credits");
        levelSelect = (Button)GetNode("ContainerButtons/Panel/MarginContainer/VBoxContainer/Levels");
        levelSelect.GrabFocus();

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
        GetTree().Quit();
    }

    private void OnShowContent(ContentType contentType)
    {
        if (contentType == ContentType.Credits)
        {
            if (!contentPanel.IsVisible())
                contentPanel.Visible = true;
            levels.SetVisible(false);
            credits.Visible = true;
            options.Visible = false;
        } else if (contentType == ContentType.Options)
        {
            if (!contentPanel.IsVisible())
                contentPanel.Visible = true;
            levels.SetVisible(false);
            credits.Visible = false;
            options.Visible = true;
        } else if (contentType == ContentType.Levels)
        {
            if (!contentPanel.IsVisible())
                contentPanel.Visible = true;
            levels.SetVisible(true);
            options.Visible = false;
            credits.Visible = false;
            levels.SetFocus();
        } else
        {
            contentPanel.Visible = false;
            levels.SetVisible(false);
            options.Visible = false;
            credits.Visible = false;
        }
    }
}
