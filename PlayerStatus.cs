using Godot;
using System;

public class PlayerStatus : CanvasLayer
{
    private Label ScoreValueLabel;
    private Label TimeValueLabel;

    public override void _Ready()
    {
        ScoreValueLabel = (Label)GetNode("ScoreContainer/ScoreValue");
        TimeValueLabel = (Label)GetNode("ScoreContainer/TimeValue");
    }

    public void UpdateScore(int score)
    {
        ScoreValueLabel.Text = score.ToString();
    }

    public void UpdatePlayed(long seconds)
    {
        TimeSpan t = TimeSpan.FromSeconds(seconds);

        TimeValueLabel.Text = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                        t.Hours,
                        t.Minutes,
                        t.Seconds);
    }
}
