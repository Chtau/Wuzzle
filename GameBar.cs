using Godot;
using System;

public class GameBar : CanvasLayer
{

    public override void _Ready()
    {

    }

    public void OnShowOptions()
    {
        var dialog = (WindowDialog)GetNode("OptionsDialog");
        dialog.ShowModal(true);
    }
}
