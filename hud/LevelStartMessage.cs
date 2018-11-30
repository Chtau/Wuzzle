using Godot;
using System;

public class LevelStartMessage : Control
{
    private MarginContainer levelStartMessage;
    private Label levelStartCountdown;
    private Action showCallback;
    private System.Timers.Timer levelTimer = new System.Timers.Timer();
    private int startCountdown = GlobalValues.LevelStartCountdown + 1;

    public override void _Ready()
    {
        levelStartMessage = (MarginContainer)GetNode("CanvasLayer/MarginContainer");
        levelStartCountdown = (Label)levelStartMessage.GetNode("VBoxContainer/Countdown");

        levelStartCountdown.Text = GlobalValues.LevelStartCountdown.ToString();

        this.Visible = true;
        levelStartMessage.Visible = true;
        levelTimer.Interval = TimeSpan.FromSeconds(1).TotalMilliseconds;
        levelTimer.Elapsed += LevelTimer_Elapsed;
        LevelTimer_Elapsed(this, null);
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
            this.Visible = false;
            showCallback.Invoke();
        }
    }
}
