using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ConfigHandler : ISingletonHandler
{
    public void LoadConfig()
    {
        GD.Print("LoadConfig");
        var config = new ConfigFile();
        var err = config.Load(GlobalValues.ConfigFilePath);
        //GD.Print("Config load Code:" + err);
        if (err == Error.CantOpen)
        {
            // generate default config file
            GetScanCodes(ref config, GlobalValues.Keymap_Move_Left);
            GetScanCodes(ref config, GlobalValues.Keymap_Move_Right);
            GetScanCodes(ref config, GlobalValues.Keymap_Move_Up);
            GetScanCodes(ref config, GlobalValues.Keymap_Move_Down);
            GetScanCodes(ref config, GlobalValues.Keymap_Move_Dash);
            GetScanCodes(ref config, GlobalValues.Keymap_Move_Jump);
            GetScanCodes(ref config, GlobalValues.Keymap_Move_Strike);
            GetScanCodes(ref config, GlobalValues.Keymap_Answer_1);
            GetScanCodes(ref config, GlobalValues.Keymap_Answer_2);
            GetScanCodes(ref config, GlobalValues.Keymap_Answer_3);

            config.Save(GlobalValues.ConfigFilePath);
        }
        else // ConfigFile was properly loaded, initialize InputMap
        {
            foreach (var actionName in config.GetSectionKeys(GlobalValues.ConfigSectionKeyInput))
            {
                var scanCode = OS.FindScancodeFromString(config.GetValue(GlobalValues.ConfigSectionKeyInput, actionName).ToString());
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

    public void GetScanCodes(ref ConfigFile config, string action)
    {
        var actionList = InputMap.GetActionList(action);
        for (int i = 0; i < actionList.Count; i++)
        {
            var inputE = (InputEventKey)actionList[i];
            var scanCode = OS.GetScancodeString(inputE.Scancode);
            config.SetValue(GlobalValues.ConfigSectionKeyInput, action, scanCode);
        }
    }

    public void SaveValue(string section, string key, object value)
    {
        var config = new ConfigFile();
        var err = config.Load(GlobalValues.ConfigFilePath);
        if (err == Error.Ok)
        {
            config.SetValue(section, key, value);
            config.Save(GlobalValues.ConfigFilePath);
        }
        else
        {
            GD.Print("OnSaveConfig Err:" + err);
        }
    }

    public void Init()
    {
        LoadConfig();
    }
}
