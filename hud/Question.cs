using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Question : CanvasLayer
{
    private List<QuestionItem> questions;
    private List<Guid> alreadyAnswered;

    private List<QuestionItem> questionQueue = new List<QuestionItem>();

    public override void _Ready()
    {
        InitQuestions();
    }

//    public override void _Process(float delta)
//    {
//        // Called every frame. Delta is time since last frame.
//        // Update game logic here.
//        
//    }

    public void ShowQuestion()
    {
        questionQueue.Add(QuestionItem());

    }

    private void InitQuestions()
    {
        questions = new List<QuestionItem>();
        alreadyAnswered = new List<Guid>();
    }

    private QuestionItem QuestionItem()
    {
        QuestionItem current = null;
        var q = questions.Where(x => !alreadyAnswered.Contains(x.Id));
        if (q.Count() == 0)
        {
            // reset the already answered questions
            alreadyAnswered.Clear();
        }
        if (q.Count() == 1)
        {
            current = q.First();
            
        } else
        {
            current = q.ElementAt(SharedFunctions.Instance.RandRand(0, q.Count()));
        }
        alreadyAnswered.Add(current.Id);
        return current;
    }
}
