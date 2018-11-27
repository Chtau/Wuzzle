using Godot;
using System;

public class MenuButton : Button
{
    private string label = "Button";
    [Export]
    public string LabelText
    {
        get { return label; }
        set
        {
            label = value;
            OnLabelChange(label);
        }
    }

    [Export]
    public PackedScene Scene { get; set; }
    
    private Label labelNode;

    public override void _Ready()
    {
        //Connect("pressed", this, nameof(OnButtonPressed));

        labelNode = (Label)GetNode("Label");
        OnLabelChange(label);
    }


    private void OnLabelChange(string text)
    {
        if (labelNode != null)
            labelNode.Text = text;
    }

    public void OnButtonPressed()
    {
        GD.Print("OnButtonPressed =>" + LabelText);
        if (Scene != null)
        {
            GD.Print("Load Scene:" + Scene.ResourcePath);
            //GetTree().ChangeScene(Scene.ResourcePath);
            SharedFunctions.Instance.LoadScene(this, Scene.ResourcePath);
        } else
        {
            GD.Print("No Scene is set for the Button");
        }
    }
}