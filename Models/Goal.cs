using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wuzzle.Models
{
    public class Goal
    {
        public Guid Id { get; set; }
        public bool Finished { get; set; }
        public string Title { get; set; }
        public int DifficultyMultiplier { get; set; }

        public Goal(Guid id, bool finished, string title)
        {
            Id = id;
            Finished = finished;
            Title = title;
            DifficultyMultiplier = 1;
        }
    }
}
