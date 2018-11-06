using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wuzzle.Models
{
    public class AnswerResult
    {
        public Guid Id { get; set; }
        public Guid GoalId { get; set; }
        public bool Status { get; set; }
        public string Result { get; set; }
    }
}
