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
    private Label answer1;
    private Label answer2;
    private Label answer3;
    private Panel panel;

    public override void _Ready()
    {
        panel = (Panel)GetNode("Panel");
        panel.Visible = false;
        InitQuestions();
        question = (Label)GetNode("/Panel/VBoxContainer/QuestionBox/QuestionLabel");
        answer1 = (Label)GetNode("/Panel/VBoxContainer/HBoxContainer/Answer1");
        answer2 = (Label)GetNode("/Panel/VBoxContainer/HBoxContainer2/Answer2");
        answer3 = (Label)GetNode("/Panel/VBoxContainer/HBoxContainer3/Answer3");
    }

    public override void _Process(float delta)
    {
        if (questionQueue.Count > 0)
        {
            if (Input.IsActionPressed(GlobalValues.Keymap_Answer_1))
            {

            }
            else if (Input.IsActionPressed(GlobalValues.Keymap_Answer_2))
            {

            }
            else if (Input.IsActionPressed(GlobalValues.Keymap_Answer_3))
            {

            }
        }
    }

    public void ShowQuestion()
    {
        questionQueue.Add(QuestionItem());

        var item = questionQueue.First();
        question.Text = item.Question;
        answer1.Text = item.Answer[0].Item1;
        answer2.Text = item.Answer[1].Item1;
        answer3.Text = item.Answer[2].Item1;

        panel.Visible = true;
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
