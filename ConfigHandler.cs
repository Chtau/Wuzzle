using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ConfigHandler : ISingletonHandler
{
    public event EventHandler InputMappingChanged;

    public void LoadConfig()
    {
        GD.Print("LoadConfig");
        var config = new ConfigFile();
        var err = config.Load(GlobalValues.ConfigFilePath);
        //GD.Print("Config load Code:" + err);
        if (err == Error.CantOpen)
        {
            // generate default config file
            CreateInput(ref config, GlobalValues.Keymap_Move_Left);
            CreateInput(ref config, GlobalValues.Keymap_Move_Right);
            CreateInput(ref config, GlobalValues.Keymap_Move_Up);
            CreateInput(ref config, GlobalValues.Keymap_Move_Down);
            CreateInput(ref config, GlobalValues.Keymap_Move_Dash);
            CreateInput(ref config, GlobalValues.Keymap_Move_Jump);
            CreateInput(ref config, GlobalValues.Keymap_Move_Strike);
            CreateInput(ref config, GlobalValues.Keymap_Answer_1);
            CreateInput(ref config, GlobalValues.Keymap_Answer_2);
            CreateInput(ref config, GlobalValues.Keymap_Answer_3);

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
            foreach (var actionName in config.GetSectionKeys(GlobalValues.ConfigSectionJoyPadInput))
            {
                var scanCode = config.GetValue(GlobalValues.ConfigSectionJoyPadInput, actionName);
                if (scanCode != null)
                {
                    var actionEvent = new InputEventJoypadButton
                    {
                        ButtonIndex = Convert.ToInt32(scanCode)
                    };
                    foreach (InputEvent oldActionEvent in InputMap.GetActionList(actionName))
                    {
                        
                        if (oldActionEvent is InputEventJoypadButton)
                        {
                            InputMap.ActionEraseEvent(actionName, oldActionEvent);
                        }
                    }
                    InputMap.ActionAddEvent(actionName, actionEvent);
                }
            }
            InputMappingChanged?.Invoke(this, new EventArgs());
        }
    }

    public void CreateInput(ref ConfigFile config, string action)
    {
        var actionList = InputMap.GetActionList(action);
        for (int i = 0; i < actionList.Count; i++)
        {
            if (actionList[i] is InputEventKey key)
            {
                var scanCode = OS.GetScancodeString(key.Scancode);
                config.SetValue(GlobalValues.ConfigSectionKeyInput, action, scanCode);
            } else if (actionList[i] is InputEventJoypadButton joypadButton)
            {
                config.SetValue(GlobalValues.ConfigSectionJoyPadInput, action, joypadButton.ButtonIndex);
            }
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

    public static string GetJoyPadStringCode(int buttonCode)
    {
        switch (buttonCode)
        {
            case 0:
                return "XBox A";
            case 1:
                return "XBox B";
            case 2:
                return "XBox X";
            case 3:
                return "XBox Y";
            case 4:
                return "L1";
            case 5:
                return "R1";
            case 6:
                return "L2";
            case 7:
                return "R2";
            case 8:
                return "L3";
            case 9:
                return "R3";
            case 10:
                return "Select";
            case 11:
                return "Start";
            case 12:
                return "D-Pad Up";
            case 13:
                return "D-Pad Down";
            case 14:
                return "D-Pad Left";
            case 15:
                return "D-Pad Right";
            default:
                return "";
        }
    }
}
