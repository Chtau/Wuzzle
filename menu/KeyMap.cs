using Godot;
using System;

public class KeyMap : Node
{
    Label inputHelper;
    CenterContainer inputHelpWrapper;

    public override void _Ready()
    {
        inputHelpWrapper = (CenterContainer)GetNode("CenterContainer");
        inputHelper = (Label)inputHelpWrapper.GetNode("Panel/InputHelper");
        inputHelpWrapper.Visible = false;
    }

    private void OnContextualHelpTextChanged(string text, bool activate)
    {
        if (activate)
        {
            inputHelper.Text = text;
            inputHelpWrapper.Visible = true;
        } else
        {
            inputHelpWrapper.Visible = false;
        }
    }

    private void OnInputSave(string actionBinding, string scanCode, bool joyPad)
    {
        if (joyPad)
            SharedFunctions.Instance.ConfigHandler.SaveValue(GlobalValues.ConfigSectionJoyPadInput, actionBinding, scanCode);
        else
            SharedFunctions.Instance.ConfigHandler.SaveValue(GlobalValues.ConfigSectionKeyInput, actionBinding, scanCode);    
    }
}