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

    public List<Wuzzle.Models.Goal> CurrentGoalList()
    {
        return new List<Wuzzle.Models.Goal>
            {
                new Wuzzle.Models.Goal(Guid.NewGuid(),false, "???"),
                new Wuzzle.Models.Goal(Guid.NewGuid(),false, "???"),
                new Wuzzle.Models.Goal(Guid.NewGuid(),false, "???"),
                new Wuzzle.Models.Goal(Guid.NewGuid(),true, "???"),
                new Wuzzle.Models.Goal(Guid.NewGuid(),false, "???"),
                new Wuzzle.Models.Goal(Guid.NewGuid(),false, "???"),
                new Wuzzle.Models.Goal(Guid.NewGuid(),false, "???"),
            };
    }

    public List<Wuzzle.Models.Hint> GoalHints(Guid goalId)
    {
        return new List<Wuzzle.Models.Hint>
            {
                new Wuzzle.Models.Hint
                {
                    GoalId = goalId,
                    Id = Guid.NewGuid(),
                    Text = "Hint1"
                },
                new Wuzzle.Models.Hint
                {
                    GoalId = goalId,
                    Id = Guid.NewGuid(),
                    Text = "Hint2"
                }
            };
    }

}
