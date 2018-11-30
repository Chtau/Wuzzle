using Godot;
using System;

public class KeyMap : Node
{
    //Label contextualHelp;
    Label inputHelper;
    CenterContainer inputHelpWrapper;

    public override void _Ready()
    {
        SharedFunctions.Instance.ConfigHandler.LoadConfig();
        inputHelpWrapper = (CenterContainer)GetNode("CenterContainer");
        inputHelper = (Label)inputHelpWrapper.GetNode("Panel/InputHelper");
        //contextualHelp = (Label)GetNode("VBoxContainer/ContextualHelp");
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
        //contextualHelp.Text = text;
    }

    private void OnInputSave(string actionBinding, string scanCode)
    {
        SharedFunctions.Instance.ConfigHandler.SaveValue(GlobalValues.ConfigSectionKeyInput, actionBinding, scanCode);
        
    }
}