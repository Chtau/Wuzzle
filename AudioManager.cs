using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AudioManager : ISingletonHandler
{
    // TODO: for SFX use only .wav format because this will not loop

    const string ConfigKeyMusic = "music";
    const string ConfigKeySFX = "sfx";

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
            OnSaveValue(ConfigKeyMusic, backgroundMusicDB);
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
            OnSaveValue(ConfigKeySFX, sfxDB);
        }
    }

    public void Init()
    {
        BackgroundMusicPosition = 0f;
        backgroundMusicDB = 0f;
        sfxDB = 0f;
        OnLoadAudio();
    }

    private void OnLoadAudio()
    {
        var config = new ConfigFile();
        var err = config.Load(GlobalValues.ConfigFilePath);
        //GD.Print("Config load Code:" + err);
        if (err == Error.CantOpen)
        {
            GD.Print("No Config file to save/load the Audio");
        }
        else // ConfigFile was properly loaded, initialize InputMap
        {
            GD.Print("Load Audio config");
            BackgroundMusicDB = (float)config.GetValue(GlobalValues.ConfigSectionAudio, ConfigKeyMusic, backgroundMusicDB);
            SFXDB = (float)config.GetValue(GlobalValues.ConfigSectionAudio, ConfigKeySFX, sfxDB);
        }
    }

    public void OnSaveValue(string key, object value)
    {
        try
        {
            var config = new ConfigFile();
            var err = config.Load(GlobalValues.ConfigFilePath);
            if (err == Error.Ok)
            {
                config.SetValue(GlobalValues.ConfigSectionAudio, key, value);
                config.Save(GlobalValues.ConfigFilePath);
            }
            else
            {
                GD.Print("OnSaveConfig Err:" + err);
            }
        } catch (Exception) { }
    }
}
