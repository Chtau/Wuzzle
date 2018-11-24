using Godot;
using System;

public class FadeIn : ColorRect
{
    [Signal]
    delegate void FadeInFinished(string foo, int bar);

    public event EventHandler FadeInFinishedEvent;

    private AnimationPlayer animationPlayer;

    public override void _Ready()
    {
        animationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");
        animationPlayer.Connect("animation_finished", this, nameof(AnimationFinished));
    }

    private void AnimationFinished(string animation)
    {
        FadeInFinishedEvent?.Invoke(this, new EventArgs());
        EmitSignal(nameof(FadeInFinished), animation);
    }

}
