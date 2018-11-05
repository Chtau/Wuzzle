using Godot;
using System;

public class Main : Node
{
    private long SecondsPlayed = 0;
    private int Score = 0;
    private Timer GameTimer;
    private PlayerStatus PlayerStatus;

    public override void _Ready()
    {
        PlayerStatus = (PlayerStatus)GetNode("PlayerStatus");

        GameTimer = new Timer
        {
            WaitTime = 1
        };
        this.AddChild(GameTimer);
        GameTimer.Connect("timeout", this, nameof(OnGameTimerTimeout));

        NewGame();
    }

    public void NewGame()
    {
        Score = 0;
        SecondsPlayed = 0;

        OnStartGame();
    }

    public void GameOver()
    {
        OnStopGame();
    }

    private void OnStartGame()
    {
        GameTimer.Start();
    }

    private void OnStopGame()
    {
        GameTimer.Stop();
    }

    private void OnGameTimerTimeout()
    {
        SecondsPlayed += 1;
        PlayerStatus.UpdatePlayed(SecondsPlayed);
    }

}