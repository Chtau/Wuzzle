using Godot;
using System;

public class InputKeyMap : HBoxContainer
{
    [Signal]
    delegate void ContextualHelpTextChanged(string text, bool activate);
    [Signal]
    delegate void InputSave(string actionBinding, string scanCode, bool joyPad);

    [Export]
    public string KeyMap { get; set; }
    [Export]
    public string KeyLabel { get; set; }

    private Button keyButton;
    private Button joyButton;

    public override void _Ready()
    {
        SharedFunctions.Instance.ConfigHandler.InputMappingChanged += ConfigHandler_InputMappingChanged;

        ((Label)GetNode("Label")).Text = KeyLabel;

        keyButton = (Button)GetNode("Button");
        joyButton = (Button)GetNode("Joy");
        keyButton.Connect("pressed", this, nameof(WaitForInput));
        joyButton.Connect("pressed", this, nameof(WaitForInputJoy));
        SetProcessInput(false);

        OnSetButtonText();
    }

    public override void _ExitTree()
    {
        SharedFunctions.Instance.ConfigHandler.InputMappingChanged -= ConfigHandler_InputMappingChanged;
        base._ExitTree();
    }

    private void ConfigHandler_InputMappingChanged(object sender, EventArgs e)
    {
        OnSetButtonText();
    }

    private void OnSetButtonText()
    {
        foreach (var item in InputMap.GetActionList(KeyMap))
        {
            if (item is InputEventKey key)
            {
                keyButton.Text = OS.GetScancodeString(key.Scancode);
            } else if (item is InputEventJoypadButton joypadButton)
            {
                joyButton.Text = ConfigHandler.GetJoyPadStringCode(joypadButton.ButtonIndex);
            } else
            {
                GD.Print("Unkown InputMap type => " + item);
            }
        }
    }

    private void WaitForInput()
    {
        EmitSignal(nameof(ContextualHelpTextChanged), "Press a key to assign to the '" + KeyLabel + "' action. (Press ESC Key to cancel)", true);
        SetProcessInput(true);
    }

    private void WaitForInputJoy()
    {
        EmitSignal(nameof(ContextualHelpTextChanged), "Press a Joypad button to assign to the '" + KeyLabel + "' action. (Press ESC Key to cancel)", true);
        SetProcessInput(true);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            GetTree().SetInputAsHandled();
            SetProcessInput(false);
            EmitSignal(nameof(ContextualHelpTextChanged), "Click a key binding to reassign it, or press the Cancel action.", false);
            if (!eventKey.IsAction("ui_cancel"))
            {
                var scanCode = OS.GetScancodeString(eventKey.Scancode);
                keyButton.Text = scanCode;
                foreach (InputEvent oldActionEvent in InputMap.GetActionList(KeyMap))
                {
                    if (oldActionEvent is InputEventKey)
                        InputMap.ActionEraseEvent(KeyMap, oldActionEvent);
                }
                InputMap.ActionAddEvent(KeyMap, eventKey);
                EmitSignal(nameof(InputSave), KeyMap, scanCode, false);
            }
        } else if (@event is InputEventJoypadButton joypadButton)
        {
            GetTree().SetInputAsHandled();
            SetProcessInput(false);
            EmitSignal(nameof(ContextualHelpTextChanged), "Click a Joypad button binding to reassign it, or press the Cancel action.", false);
            if (!joypadButton.IsAction("ui_cancel"))
            {
                var scanCode = ConfigHandler.GetJoyPadStringCode(joypadButton.ButtonIndex);
                joyButton.Text = scanCode;
                foreach (InputEvent oldActionEvent in InputMap.GetActionList(KeyMap))
                {
                    if (oldActionEvent is InputEventJoypadButton)
                        InputMap.ActionEraseEvent(KeyMap, oldActionEvent);
                }
                InputMap.ActionAddEvent(KeyMap, joypadButton);
                EmitSignal(nameof(InputSave), KeyMap, joypadButton.ButtonIndex, true);
            }
        }
    }

}
