using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LevelItem : LevelUserItem
{
    public new Guid Id { get; set; }
    public int Order { get; set; }
    public string LevelTitle { get; set; }
    public string LevelDescription { get; set; }
    public string ScenePath { get; set; }
    public int RequieredQuestions { get; set; }
    public TimeSpan GoldTime { get; set; }
    public TimeSpan SilverTime { get; set; }
    public TimeSpan BronzeTime { get; set; }

}
