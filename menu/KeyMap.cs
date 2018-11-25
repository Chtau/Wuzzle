using Godot;
using System;

public class KeyMap : Node
{
    Label contextualHelp;

    public override void _Ready()
    {
        SharedFunctions.Instance.ConfigHandler.LoadConfig();
        contextualHelp = (Label)GetNode("MarginContainer/VBoxContainer/ContextualHelp");
    }

    private void OnContextualHelpTextChanged(string text)
    {
        contextualHelp.Text = text;
    }

    private void OnInputSave(string actionBinding, string scanCode)
    {
        SharedFunctions.Instance.ConfigHandler.SaveValue(GlobalValues.ConfigSectionKeyInput, actionBinding, scanCode);
        
    }
}