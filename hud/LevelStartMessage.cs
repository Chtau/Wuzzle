using Godot;
using System;

public class LevelStartMessage : CanvasLayer
{
    private MarginContainer levelStartMessage;
    private Label levelStartCountdown;
    private Action showCallback;
    private System.Timers.Timer levelTimer = new System.Timers.Timer();
    private int startCountdown = GlobalValues.LevelStartCountdown + 1;

    public override void _Ready()
    {
        levelStartMessage = (MarginContainer)GetNode("MarginContainer");
        levelStartCountdown = (Label)GetNode("MarginContainer/VBoxContainer/Countdown");

        levelTimer.Interval = TimeSpan.FromSeconds(1).TotalMilliseconds;
        levelTimer.Elapsed += LevelTimer_Elapsed;
    }

    public void Show(Action callback)
    {
        showCallback = callback;

        levelTimer.Start();
    }

    private void LevelTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        startCountdown--;
        if (!levelStartMessage.IsVisible())
            levelStartMessage.Visible = true;
        levelStartCountdown.Text = startCountdown.ToString();
        if (startCountdown < 0)
        {
            levelTimer.Stop();
            levelStartMessage.Visible = false;
            showCallback.Invoke();
        }
    }
}
