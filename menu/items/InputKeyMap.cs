using Godot;
using System;

public class InputKeyMap : HBoxContainer
{
    [Signal]
    delegate void ContextualHelpTextChanged(string text);
    [Signal]
    delegate void InputSave(string actionBinding, string scanCode);

    [Export]
    public string KeyMap { get; set; }
    [Export]
    public string KeyLabel { get; set; }

    Button keyButton;

    public override void _Ready()
    {
        ((Label)GetNode("Label")).Text = KeyLabel;

        InputEventKey inputEvent = (InputEventKey)InputMap.GetActionList(KeyMap)[0];
        keyButton = (Button)GetNode("Button");
        keyButton.Text = OS.GetScancodeString(inputEvent.Scancode);
        keyButton.Connect("pressed", this, nameof(WaitForInput));
        SetProcessInput(false);
    }

    private void WaitForInput()
    {
        //actionBinding = binding;
        //keyButton = (Button)GetNode(actionBinding);
        EmitSignal(nameof(ContextualHelpTextChanged), "Press a key to assign to the '" + KeyLabel + "' action.");
        //((Label)GetNode("contextualHelp")).Text = "Press a key to assign to the '" + KeyLabel + "' action.";
        SetProcessInput(true);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey eventKey)
        {
            GetTree().SetInputAsHandled();
            SetProcessInput(false);
            EmitSignal(nameof(ContextualHelpTextChanged), "Click a key binding to reassign it, or press the Cancel action.");
            //((Label)GetNode("contextualHelp")).Text = "Click a key binding to reassign it, or press the Cancel action.";
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
                EmitSignal(nameof(InputSave), KeyMap, scanCode);
                // OnSaveConfig(SectionKeyInput, actionBinding, scanCode);
            }
        }
    }

}
