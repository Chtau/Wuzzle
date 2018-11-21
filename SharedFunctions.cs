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
    }

    public static SharedFunctions Instance { get; } = new SharedFunctions();
    #endregion

    private Random rand = new Random();

    public int RandRand(int min, int max)
    {
        return (int)(rand.NextDouble() * (max - min) + min);
    }

    public float RandRand(float min, float max)
    {
        return (float)(rand.NextDouble() * (max - min) + min);
    }

    public FileHandler FileHandler { get; set; }
}