using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wuzzle.Models
{
    public class Hint
    {
        public Guid Id { get; set; }
        public Guid GoalId { get; set; }
        public string Text { get; set; }
    }
}
