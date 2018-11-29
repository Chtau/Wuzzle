using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class QuestionItem
{
    public Guid Id { get; set; }
    public string Question { get; set; }
    public List<Tuple<string, bool>> Answer { get; set; }
    public float Seconds { get; set; }
    public QuestionManager.Difficulty Difficulty { get; set; }
    public QuestionManager.Categorie Categorie { get; set; }

    public QuestionItem()
    {
        Id = Guid.NewGuid();
        Question = null;
        Answer = new List<Tuple<string, bool>>();
        Seconds = 30;
        Difficulty = QuestionManager.Difficulty.Easy;
        Categorie = QuestionManager.Categorie.Math;
    }
}
