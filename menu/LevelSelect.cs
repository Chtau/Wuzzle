using Godot;
using System;
using System.Linq;

public class LevelSelect : Control
{
    [Export]
    public PackedScene PackedScene;

    private VBoxContainer scrollWrapper;
    private VBoxContainer container;

    public override void _Ready()
    {
        SharedFunctions.Instance.LevelManager.Init();

        container = (VBoxContainer)GetNode("VBoxContainer");
        scrollWrapper = (VBoxContainer)GetNode("VBoxContainer/ScrollContainer/ScrollWrapper");
        scrollWrapper.RemoveChild(scrollWrapper.GetChild(0));

        foreach (LevelItem item in SharedFunctions.Instance.LevelManager.Levels.OrderBy(x => x.Order))
        {

            LevelSelectItem select = (LevelSelectItem)PackedScene.Instance();
            select.ChangeLevelItem(item);
            scrollWrapper.AddChild(select);
        }
    }
}