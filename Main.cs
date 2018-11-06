using Godot;
using System;

public class Main : Node
{
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

        Wuzzle.Profil.ProfilManager.Instance.ScoreChanged += Profil_ScoreChanged;

        NewGame();
    }

    public void NewGame()
    {
        Wuzzle.Profil.ProfilManager.Instance.Load();

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
        Wuzzle.Profil.ProfilManager.Instance.AddPlayedSeconds(1);
        PlayerStatus.UpdatePlayed(Wuzzle.Profil.ProfilManager.Instance.PlayedSeconds);
    }

    private void Profil_ScoreChanged(object sender, int e)
    {
        PlayerStatus.UpdateScore(e);
    }

}