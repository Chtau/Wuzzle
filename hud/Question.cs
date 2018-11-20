using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Question : CanvasLayer
{
    private List<QuestionItem> questions;
    private List<Guid> alreadyAnswered;

    private List<QuestionItem> questionQueue = new List<QuestionItem>();

    private Label question;
    private Answer answer1;
    private Answer answer2;
    private Answer answer3;
    private Panel panel;

    public override void _Ready()
    {
        panel = (Panel)GetNode("Panel");
        panel.Visible = false;
        InitQuestions();
        question = (Label)GetNode("/Panel/VBoxContainer/QuestionBox/QuestionLabel");
        answer1 = (Answer)GetNode("/Panel/VBoxContainer/AnswerBox");
        answer2 = (Answer)GetNode("/Panel/VBoxContainer/AnswerBox2");
        answer3 = (Answer)GetNode("/Panel/VBoxContainer/AnswerBox3");
    }

    public override void _Process(float delta)
    {
        if (questionQueue.Count > 0)
        {
            if (Input.IsActionPressed(GlobalValues.Keymap_Answer_1))
            {
                HandleAnswer(answer1.IsCorrect);
            }
            else if (Input.IsActionPressed(GlobalValues.Keymap_Answer_2))
            {
                HandleAnswer(answer2.IsCorrect);
            }
            else if (Input.IsActionPressed(GlobalValues.Keymap_Answer_3))
            {
                HandleAnswer(answer3.IsCorrect);
            }
        }
    }

    public void ShowQuestion()
    {
        questionQueue.Add(QuestionItem());

        var item = questionQueue.First();
        question.Text = item.Question;
        answer1.SetAnswer(item.Answer[0].Item1, item.Answer[0].Item2);
        answer2.SetAnswer(item.Answer[1].Item1, item.Answer[1].Item2);
        answer3.SetAnswer(item.Answer[2].Item1, item.Answer[2].Item2);

        panel.Visible = true;
    }

    private void HandleAnswer(bool result)
    {
        GD.Print("Answer result:" + result);
        alreadyAnswered.Add(questionQueue.First().Id);
        questions.RemoveAt(0);
        questionQueue.RemoveAt(0);

        if (questionQueue.Count > 0)
        {
            // show the next question
            ShowQuestion();
        } else
        {
            panel.Visible = false;
        }
    }

    private void InitQuestions()
    {
        questions = new List<QuestionItem>();
        questions.Add(new QuestionItem
        {
            Question = "Calculate the sum for '12 * 3 + 7'",
            Answer = new List<Tuple<string, bool>>
            {
                new Tuple<string, bool>("43", true),
                new Tuple<string, bool>("67", false),
                new Tuple<string, bool>("50", false)
            }
        });
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
