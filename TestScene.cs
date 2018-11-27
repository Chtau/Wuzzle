using Godot;
using System;

public class TestScene : Control
{
    
    private void _on_Button_pressed()
    {
        SharedFunctions.Instance.LoadScene(this, GlobalValues.MainMenuScene);
    }
}



