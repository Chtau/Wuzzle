using Godot;
using System;

public class KeyMap : Node
{
    const string ConfigFilePath = "user://config.cfg";
    const string SectionKeyInput = "input";

    Label contextualHelp;

    public override void _Ready()
    {
        OnLoadConfig();
        contextualHelp = (Label)GetNode("MarginContainer/VBoxContainer/ContextualHelp");
    }


    private void OnLoadConfig()
    {
        var config = new ConfigFile();
        var err = config.Load(ConfigFilePath);
        //GD.Print("Config load Code:" + err);
        if (err == Error.CantOpen)
        {
            // generate default config file
            OnGetScanCodes(ref config, GlobalValues.Keymap_Move_Left);
            OnGetScanCodes(ref config, GlobalValues.Keymap_Move_Right);
            OnGetScanCodes(ref config, GlobalValues.Keymap_Move_Up);
            OnGetScanCodes(ref config, GlobalValues.Keymap_Move_Down);
            OnGetScanCodes(ref config, GlobalValues.Keymap_Move_Dash);
            OnGetScanCodes(ref config, GlobalValues.Keymap_Move_Jump);
            OnGetScanCodes(ref config, GlobalValues.Keymap_Move_Strike);
            OnGetScanCodes(ref config, GlobalValues.Keymap_Answer_1);
            OnGetScanCodes(ref config, GlobalValues.Keymap_Answer_2);
            OnGetScanCodes(ref config, GlobalValues.Keymap_Answer_3);

            config.Save(ConfigFilePath);
        }
        else // ConfigFile was properly loaded, initialize InputMap
        {
            foreach (var actionName in config.GetSectionKeys(SectionKeyInput))
            {
                var scanCode = OS.FindScancodeFromString(config.GetValue(SectionKeyInput, actionName).ToString());
                var actionEvent = new InputEventKey
                {
                    Scancode = scanCode
                };
                foreach (InputEvent oldActionEvent in InputMap.GetActionList(actionName))
                {
                    if (oldActionEvent is InputEventKey)
                        InputMap.ActionEraseEvent(actionName, oldActionEvent);
                }
                InputMap.ActionAddEvent(actionName, actionEvent);
            }
        }
    }

    private void OnGetScanCodes(ref ConfigFile config, string action)
    {
        var actionList = InputMap.GetActionList(action);
        for (int i = 0; i < actionList.Count; i++)
        {
            var inputE = (InputEventKey)actionList[i];
            var scanCode = OS.GetScancodeString(inputE.Scancode);
            config.SetValue(SectionKeyInput, action, scanCode);
        }
    }

    /*private void OnSaveConfig(string section, string key, object value)
    {
        var config = new ConfigFile();
        var err = config.Load(ConfigFilePath);
        if (err == Error.Ok)
        {
            config.SetValue(section, key, value);
            config.Save(ConfigFilePath);
        } else
        {
            GD.Print("OnSaveConfig Err:" + err);
        }
    }*/

    private void OnContextualHelpTextChanged(string text)
    {
        contextualHelp.Text = text;
    }


    private void OnInputSave(string actionBinding, string scanCode)
    {
        var config = new ConfigFile();
        var err = config.Load(ConfigFilePath);
        if (err == Error.Ok)
        {
            config.SetValue(SectionKeyInput, actionBinding, scanCode);
            config.Save(ConfigFilePath);
        }
        else
        {
            GD.Print("OnSaveConfig Err:" + err);
        }
    }
}


