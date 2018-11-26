using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameState
{
    public event EventHandler<float> CurrentLifeChanged;
    public event EventHandler<float> MaxLifeChanged;
    public event EventHandler<TimeSpan> LevelTimeChanged;

    private float currentLife;
    public float CurrentLife
    {
        get { return currentLife; }
        set
        {
            currentLife = value;
            CurrentLifeChanged?.Invoke(this, currentLife);
        }
    }
    private float maxLife;
    public float MaxLife
    {
        get { return maxLife; }
        set
        {
            maxLife = value;
            MaxLifeChanged?.Invoke(this, maxLife);
        }
    }
    public TimeSpan GameTime { get; set; }
    private TimeSpan levelTime;
    public TimeSpan LevelTime
    {
        get { return levelTime; }
        set
        {
            levelTime = value;
            LevelTimeChanged?.Invoke(this, levelTime);
        }
    }
}
