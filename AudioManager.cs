using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AudioManager : ISingletonHandler
{
    public float BackgroundMusicPosition { get; set; }
    public event EventHandler<float> BackgroundMusicDBChanged;
    public event EventHandler<float> SFXDBChanged;

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

    private float sfxDB;
    public float SFXDB
    {
        get { return sfxDB; }
        set
        {
            sfxDB = value;
            SFXDBChanged?.Invoke(this, sfxDB);
        }
    }

    public void Init()
    {
        BackgroundMusicPosition = 0f;
        backgroundMusicDB = 0f;
        sfxDB = 0f;
    }
}
