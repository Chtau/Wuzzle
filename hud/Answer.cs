using Godot;
using System;

public class Answer : HBoxContainer
{
    private Label answer;
    private string text = null;

    public bool IsCorrect { get; private set; }
    public Guid QuestionId { get; set; }

    public override void _Ready()
    {
        answer = (Label)GetNode("Answer");
        if (!string.IsNullOrWhiteSpace(text))
            answer.Text = text;
    }

    public void SetAnswer(string text, bool isCorrect)
    {
        this.text = text;
        if (answer != null)
        {
            answer.Text = text;
        }
        IsCorrect = isCorrect;
    }

}
