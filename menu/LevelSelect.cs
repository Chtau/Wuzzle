using Godot;
using System;
using System.Linq;

public class LevelSelect : Node
{
    [Export]
    public PackedScene PackedScene;

    private VBoxContainer scrollWrapper;

    public override void _Ready()
    {
        SharedFunctions.Instance.LevelManager.Init();

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