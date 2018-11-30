using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class SharedFunctions
{
    #region Singleton

    static SharedFunctions()
    {
    }

    private SharedFunctions()
    {
        FileHandler = new FileHandler();
        ConfigHandler = new ConfigHandler();
        GameState = new GameState();
        LevelManager = new LevelManager();
        AudioManager = new AudioManager();
        QuestionManager = new QuestionManager();
    }

    public static SharedFunctions Instance { get; } = new SharedFunctions();
    #endregion

    private bool initLoaded = false;
    public void Init()
    {
        if (!initLoaded)
        {
            FileHandler.Init();
            ConfigHandler.Init();
            GameState.Init();
            LevelManager.Init();
            AudioManager.Init();
            QuestionManager.Init();
            initLoaded = true;
        }
    }

    private Random rand = new Random();

    public int RandRand(int min, int max)
    {
        return (int)(rand.NextDouble() * (max - min) + min);
    }

    public float RandRand(float min, float max)
    {
        return (float)(rand.NextDouble() * (max - min) + min);
    }

    public string FormatTimeSpan(TimeSpan timeSpan)
    {
        return timeSpan.ToString("mm':'ss':'fff");
    }

    public FileHandler FileHandler { get; set; }
    public ConfigHandler ConfigHandler { get; set; }

    public void LoadScene(Node instance, string scenePath)
    {
        if (instance.GetTree().IsPaused())
            instance.GetTree().Paused = false;
        instance.GetTree().ChangeScene(scenePath);
    }

    public GameState GameState { get; set; }
    public LevelManager LevelManager { get; set; }
    public AudioManager AudioManager { get; set; }
    public QuestionManager QuestionManager { get; set; }
}