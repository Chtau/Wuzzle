using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class Question : Control
{
    private List<QuestionItem> questionQueue = new List<QuestionItem>();
    private Guid currentQuestionId = Guid.Empty;

    private Label question;
    private Answer answer1;
    private Answer answer2;
    private Answer answer3;
    private NinePatchRect panel;
    private TextureProgress questionTimeout;
    private Label questionTimeoutText;
    private CenterContainer answerHandleMessage;
    private Label correctAnswer;
    private Label wrongAnswer;
    private VBoxContainer wrongAnswerDetail;
    private Label wrongAnswerDetailQuestion;
    private Label wrongAnswerDetailAnswer;

    private System.Timers.Timer timer = new System.Timers.Timer();
    private TimeSpan questionTime = new TimeSpan();
    private System.Timers.Timer timerQueuedQuestions = new System.Timers.Timer();
    private System.Timers.Timer answerResult = new System.Timers.Timer();

    public override void _Ready()
    {
        panel = (NinePatchRect)GetNode("CanvasLayer/Panel");
        panel.Visible = false;
        this.Visible = false;
        question = (Label)panel.GetNode("VBoxContainer/QuestionLabel");
        answer1 = (Answer)panel.GetNode("VBoxContainer/AnswerBox");
        answer2 = (Answer)panel.GetNode("VBoxContainer/AnswerBox2");
        answer3 = (Answer)panel.GetNode("VBoxContainer/AnswerBox3");
        questionTimeout = (TextureProgress)panel.GetNode("VBoxContainer/MarginContainer/HBoxContainer/TextureProgress");
        questionTimeoutText = (Label)panel.GetNode("VBoxContainer/MarginContainer/HBoxContainer/Label");

        answerHandleMessage = (CenterContainer)GetNode("CanvasLayer/AnswerHandleMessage");
        answerHandleMessage.Visible = false;
        correctAnswer = (Label)answerHandleMessage.GetNode("VBoxContainer/CorrectAnswer");
        correctAnswer.Visible = false;
        wrongAnswer = (Label)answerHandleMessage.GetNode("VBoxContainer/WrongAnswer");
        wrongAnswer.Visible = false;
        wrongAnswerDetail = (VBoxContainer)answerHandleMessage.GetNode("VBoxContainer/WrongAnswerDetail");
        wrongAnswerDetail.Visible = false;
        wrongAnswerDetailQuestion = (Label)wrongAnswerDetail.GetNode("Question");
        wrongAnswerDetailAnswer = (Label)wrongAnswerDetail.GetNode("Answer");


        timer.Interval = new TimeSpan(0, 0, 1).TotalMilliseconds;
        timer.Elapsed += Timer_Elapsed;
        timerQueuedQuestions.Interval = new TimeSpan(0, 0, 1).TotalMilliseconds;
        timerQueuedQuestions.Elapsed += TimerQueuedQuestions_Elapsed;
        answerResult.Interval = new TimeSpan(0, 0, 2).TotalMilliseconds;
        answerResult.Elapsed += AnswerResult_Elapsed;
    }

    private void AnswerResult_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        ResetQuestion(false);
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
                ResetQuestion(false);
                HandleAnswer(question.Id, false);
            }
            else
            {
                questionTime = questionTime.Add(new TimeSpan(0, 0, 1));
                questionTimeout.Value -= 1;
                questionTimeoutText.Text = QuestionTimeText(questionTimeout.Value);
            }
        } else
        {
            ResetQuestion(false);
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
        //only call show when we are not in a waiting phase
        if (!answerResult.Enabled)
            OnShowQuestion();
    }

    public void ClearQuestions()
    {
        currentQuestionId = Guid.Empty;
        questionQueue.Clear();
        ResetQuestion(false);
    }

    private void OnShowQuestion()
    {
        var item = ActiveQuestion();
        if (item == null)
        {
            ResetQuestion(false);
            return;
        }
        question.Text = item.Question;
        answer1.QuestionId = item.Id;
        answer1.SetAnswer(OnAnswerPrefix(GlobalValues.Keymap_Answer_1) + item.Answer[0].Item1, item.Answer[0].Item2);
        answer2.QuestionId = item.Id;
        answer2.SetAnswer(OnAnswerPrefix(GlobalValues.Keymap_Answer_2) + item.Answer[1].Item1, item.Answer[1].Item2);
        answer3.QuestionId = item.Id;
        answer3.SetAnswer(OnAnswerPrefix(GlobalValues.Keymap_Answer_3) + item.Answer[2].Item1, item.Answer[2].Item2);

        questionTimeout.MaxValue = item.Seconds;
        questionTimeout.Value = item.Seconds;
        questionTimeoutText.Text = QuestionTimeText(questionTimeout.Value);

        panel.Visible = true;
        this.Visible = true;
        timer.Start();
    }
    
    private string OnAnswerPrefix(string keyMap)
    {
        InputEventKey inputEvent = (InputEventKey)InputMap.GetActionList(keyMap)[0];
        return OS.GetScancodeString(inputEvent.Scancode) + ".) ";
    }

    private string QuestionTimeText(float time)
    {
        return time + " seconds left";
    }

    private void HandleAnswer(Guid questionId, bool result)
    {
        if (SharedFunctions.Instance.QuestionManager.AlreadyAnswered(questionId))
            return;
        if (result)
        {
            SharedFunctions.Instance.GameState.LevelAnsweredQuestions++;
        } else
        {
            SharedFunctions.Instance.GameState.LevelAnsweredQuestionsWrong++;
        }
        var question = ActiveQuestion();
        string questionText = question.Question;
        string correctAnswer = question.Answer.First(x => x.Item2).Item1;
        SharedFunctions.Instance.QuestionManager.AnsweredQuestion(question);
        questionQueue.Remove(question);
        currentQuestionId = Guid.Empty;

        GD.Print("questionQueue =>" + questionQueue.Count);
        if (questionQueue.Count > 0)
        {
            // show the next question form the queue
            currentQuestionId = questionQueue.First().Id;
            ResetQuestion(result, questionText, correctAnswer);
        } else
        {
            ResetQuestion(result, questionText, correctAnswer);
        }
    }

    private void ResetQuestion(bool correct, string question = null, string answer = null)
    {
        questionTime = new TimeSpan();
        if (!string.IsNullOrWhiteSpace(question))
        {
            panel.Visible = false;
            OnShowAnsweredInfo(correct, question, answer);
            answerResult.Start();
        } else
        {
            panel.Visible = false;
            this.Visible = false;
            answerHandleMessage.Visible = false;
            correctAnswer.Visible = false;
            wrongAnswer.Visible = false;
            wrongAnswerDetail.Visible = false;
            answerResult.Stop();
            timerQueuedQuestions.Start();
        }
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

    private void OnShowAnsweredInfo(bool correct, string question = null, string answer = null)
    {
        if (correct)
        {
            answerHandleMessage.Visible = true;
            correctAnswer.Visible = true;
            wrongAnswer.Visible = false;
            wrongAnswerDetail.Visible = false;
        } else
        {
            if (!string.IsNullOrWhiteSpace(question))
            {
                wrongAnswerDetailQuestion.Text = "Question: " + question;
                wrongAnswerDetailAnswer.Text = "Correct answer: " + answer;
                answerHandleMessage.Visible = true;
                correctAnswer.Visible = false;
                wrongAnswer.Visible = true;
                wrongAnswerDetail.Visible = true;
            }
        }
    }
}
