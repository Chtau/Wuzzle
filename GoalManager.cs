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

    public Wuzzle.Models.AnswerResult VerifyAnswer(Wuzzle.Models.Goal goal, string answer)
    {
        if (answer == "")
            return new Wuzzle.Models.AnswerResult
            {
                GoalId = goal.Id,
                Id = Guid.NewGuid(),
                Result = "Empty answer",
                Status = false
            };
        Wuzzle.Profil.ProfilManager.Instance.AddScore(100 * goal.DifficultyMultiplier);
        return new Wuzzle.Models.AnswerResult
        {
            GoalId = goal.Id,
            Id = Guid.NewGuid(),
            Result = "Congratz",
            Status = true
        };
    }

}
