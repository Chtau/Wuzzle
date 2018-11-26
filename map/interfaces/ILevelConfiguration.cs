using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ILevelConfiguration
{
    int RequieredQuestions { get; }
    TimeSpan GoldTime { get; }
    TimeSpan SilverTime { get; }
    TimeSpan BronzeTime { get; }
}