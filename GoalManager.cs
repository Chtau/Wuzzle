using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class GoalManager
{

    #region Singleton

    static GoalManager()
    {
    }

    private GoalManager()
    {
    }

    public static GoalManager Instance { get; } = new GoalManager();
    #endregion

    public List<Tuple<Guid, bool, string>> CurrentGoalList()
    {
        return new List<Tuple<Guid, bool, string>>
            {
                new Tuple<Guid, bool, string>(Guid.NewGuid(),false, "???"),
                new Tuple<Guid, bool, string>(Guid.NewGuid(),false, "???"),
                new Tuple<Guid, bool, string>(Guid.NewGuid(),false, "???"),
                new Tuple<Guid, bool, string>(Guid.NewGuid(),true, "???"),
                new Tuple<Guid, bool, string>(Guid.NewGuid(),false, "???"),
                new Tuple<Guid, bool, string>(Guid.NewGuid(),false, "???"),
                new Tuple<Guid, bool, string>(Guid.NewGuid(),false, "???"),
            };
    }

    public List<string> GoalHints(Guid goalId)
    {
        return new List<string>
            {
                "Hint1",
                "Hint2",
            };
    }

}
