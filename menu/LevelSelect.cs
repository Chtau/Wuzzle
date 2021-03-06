using Godot;
using System;
using System.Linq;

public class LevelSelect : Control
{
    [Export]
    public PackedScene PackedScene;

    private VBoxContainer scrollWrapper;
    private VBoxContainer container;
    private LevelSelectItem firstItem = null;

    public override void _Ready()
    {
        //SharedFunctions.Instance.LevelManager.Init();
        SharedFunctions.Instance.Init();
        container = (VBoxContainer)GetNode("VBoxContainer");
        scrollWrapper = (VBoxContainer)GetNode("VBoxContainer/ScrollContainer/ScrollWrapper");
        int childs = scrollWrapper.GetChildCount();
        for (int i = 0; i < childs; i++)
        {
            scrollWrapper.RemoveChild(scrollWrapper.GetChild(0));
        }

        foreach (LevelItem item in SharedFunctions.Instance.LevelManager.Levels.OrderBy(x => x.Order))
        {
            LevelSelectItem select = (LevelSelectItem)PackedScene.Instance();
            select.ChangeLevelItem(item);
            if (firstItem == null)
                firstItem = select;
            scrollWrapper.AddChild(select);
        }
    }

    public void SetFocus()
    {
        if (firstItem != null)
        {
            firstItem.SetFocus();
        }
    }
}