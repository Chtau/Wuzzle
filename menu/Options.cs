using Godot;
using System;

public class Options : Node
{
    
    public override void _Ready()
    {
        
    }

    private void OnBackPressed()
    {
        SharedFunctions.Instance.LoadScene(this, "res://menu/StartMenu.tscn");
    }
}