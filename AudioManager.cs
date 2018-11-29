using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AudioManager : ISingletonHandler
{
    public float BackgroundMusicPosition { get; set; }
    public event EventHandler<float> BackgroundMusicDBChanged;

    private float backgroundMusicDB;
    public float BackgroundMusicDB
    {
        get { return backgroundMusicDB; }
        set
        {
            backgroundMusicDB = value;
            BackgroundMusicDBChanged?.Invoke(this, backgroundMusicDB);
        }
    }

    public void Init()
    {
        BackgroundMusicPosition = 0f;
        backgroundMusicDB = 0f;
    }
}
