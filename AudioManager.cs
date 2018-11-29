using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AudioManager : ISingletonHandler
{
    public float BackgroundMusicPosition { get; set; }

    public void Init()
    {
        BackgroundMusicPosition = 0f;
    }
}
