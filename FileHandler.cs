using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class FileHandler
{
    public void Save<T>(T content, string path, string pass = null)
    {
        using (var file = new File())
        {
            if (!string.IsNullOrWhiteSpace(pass))
                file.OpenEncryptedWithPass($"user://{path}", (int)File.ModeFlags.Write, pass);
            else
                file.Open($"user://{path}", (int)File.ModeFlags.Write);
            
            file.StoreString(Newtonsoft.Json.JsonConvert.SerializeObject(content));
            GD.Print("Save Path:" + file.GetPathAbsolute());
            file.Close();
        }
    }

    public T Load<T>(string path, string pass = null)
    {
        T retVal = default;
        using (var file = new File())
        {
            if (!file.FileExists($"user://{path}"))
                return default;

            if (!string.IsNullOrWhiteSpace(pass))
                file.OpenEncryptedWithPass($"user://{path}", (int)File.ModeFlags.Read, pass);
            else
                file.Open($"user://{path}", (int)File.ModeFlags.Read);
            retVal = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(file.GetAsText());
            GD.Print("Load Path:" + file.GetPathAbsolute());
            file.Close();
        }
        return retVal;
    }
}