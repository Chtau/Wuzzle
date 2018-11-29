using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Question : CanvasLayer
{
    private List<QuestionItem> questionQueue = new List<QuestionItem>();
    private Guid currentQuestionId = Guid.Empty;

    private Label question;
    private Answer answer1;
    private Answer answer2;
    private Answer answer3;
    private Panel panel;
    private ProgressBar questionTimeout;
    private Label questionTimeoutText;

    private System.Timers.Timer timer = new System.Timers.Timer();
    private TimeSpan questionTime = new TimeSpan();
    private System.Timers.Timer timerQueuedQuestions = new System.Timers.Timer();

    public override void _Ready()
    {
        panel = (Panel)GetNode("Panel");
        panel.Visible = false;
        question = (Label)GetNode("Panel/VBoxContainer/QuestionBox/QuestionLabel");
        answer1 = (Answer)GetNode("Panel/VBoxContainer/AnswerBox");
        answer2 = (Answer)GetNode("Panel/VBoxContainer/AnswerBox2");
        answer3 = (Answer)GetNode("Panel/VBoxContainer/AnswerBox3");
        questionTimeout = (ProgressBar)GetNode("Panel/VBoxContainer/HBoxContainer/ProgressBar");
        questionTimeoutText = (Label)GetNode("Panel/VBoxContainer/HBoxContainer/Label");

        timer.Interval = new TimeSpan(0, 0, 1).TotalMilliseconds;
        timer.Elapsed += Timer_Elapsed;
        timerQueuedQuestions.Interval = new TimeSpan(0, 0, 1).TotalMilliseconds;
        timerQueuedQuestions.Elapsed += TimerQueuedQuestions_Elapsed;
    }

    private void TimerQueuedQuestions_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        OnShowQuestion();
        timerQueuedQuestions.Stop();
    }

    private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        var question = ActiveQuestion();
        if (question != null)
        {
            if (questionTime.TotalSeconds >= (question.Seconds - 1))
            {
                questionTimeout.Value = 0;
                questionTimeoutText.Text = QuestionTimeText(questionTimeout.Value);
                ResetQuestion();
                questionQueue.Remove(question);
                currentQuestionId = Guid.Empty;
            }
            else
            {
                questionTime = questionTime.Add(new TimeSpan(0, 0, 1));
                questionTimeout.Value -= 1;
                questionTimeoutText.Text = QuestionTimeText(questionTimeout.Value);
            }
        } else
        {
            ResetQuestion();
            currentQuestionId = Guid.Empty;
        }
    }

    public override void _Process(float delta)
    {
        if (ActiveQuestion() != null)
        {
            if (Input.IsActionPressed(GlobalValues.Keymap_Answer_1))
            {
                HandleAnswer(answer1.QuestionId, answer1.IsCorrect);
            }
            else if (Input.IsActionPressed(GlobalValues.Keymap_Answer_2))
            {
                HandleAnswer(answer2.QuestionId, answer2.IsCorrect);
            }
            else if (Input.IsActionPressed(GlobalValues.Keymap_Answer_3))
            {
                HandleAnswer(answer3.QuestionId, answer3.IsCorrect);
            }
        }
    }

    public void AddShowQuestion()
    {
        var newQuestion = SharedFunctions.Instance.QuestionManager.Question();
        questionQueue.Add(newQuestion);

        if (currentQuestionId == Guid.Empty)
            currentQuestionId = newQuestion.Id;
        else
            return;

        OnShowQuestion();
    }

    private void OnShowQuestion()
    {
        var item = ActiveQuestion();
        if (item == null)
        {
            ResetQuestion();
            return;
        }
        question.Text = item.Question;
        answer1.QuestionId = item.Id;
        answer1.SetAnswer(item.Answer[0].Item1, item.Answer[0].Item2);
        answer2.QuestionId = item.Id;
        answer2.SetAnswer(item.Answer[1].Item1, item.Answer[1].Item2);
        answer3.QuestionId = item.Id;
        answer3.SetAnswer(item.Answer[2].Item1, item.Answer[2].Item2);

        questionTimeout.MaxValue = item.Seconds;
        questionTimeout.Value = item.Seconds;
        questionTimeoutText.Text = QuestionTimeText(questionTimeout.Value);

        panel.Visible = true;
        timer.Start();
    }

    private string QuestionTimeText(float time)
    {
        return time + " seconds left";
    }

    private void HandleAnswer(Guid questionId, bool result)
    {
        if (SharedFunctions.Instance.QuestionManager.AlreadyAnswered(questionId))
            return;
        GD.Print("Answer result:" + result);
        if (result)
        {
            SharedFunctions.Instance.GameState.LevelAnsweredQuestions++;
        }
        var question = ActiveQuestion();
        SharedFunctions.Instance.QuestionManager.AnsweredQuestion(question);
        questionQueue.Remove(question);
        currentQuestionId = Guid.Empty;

        GD.Print("questionQueue =>" + questionQueue.Count);
        if (questionQueue.Count > 0)
        {
            // show the next question form the queue
            currentQuestionId = questionQueue.First().Id;
            ResetQuestion();
            timerQueuedQuestions.Start();
        } else
        {
            ResetQuestion();
        }
    }

    private void ResetQuestion()
    {
        panel.Visible = false;
        timer.Stop();
    }

    private QuestionItem ActiveQuestion()
    {
        if (currentQuestionId != Guid.Empty && questionQueue != null && questionQueue.Any(x => x.Id == currentQuestionId))
        {
            return questionQueue.First(x => x.Id == currentQuestionId);
        }
        return null;
    }
}
