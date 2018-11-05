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

        string value = "";
        if (t.TotalHours >= 1)
        {
            value = string.Format("{0:D2}h:{1:D2}m:{2:D2}s",
                        t.Hours,
                        t.Minutes,
                        t.Seconds);
        } else if (t.TotalMinutes >= 1)
        {
            value = string.Format("{0:D2}m:{1:D2}s",
                        t.Minutes,
                        t.Seconds);
        } else
        {
            value = string.Format("{0:D2}s",
                        t.Seconds);
        }

        TimeValueLabel.Text = value;
    }
}
