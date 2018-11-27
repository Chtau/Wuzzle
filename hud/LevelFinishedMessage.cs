using Godot;
using System;
using System.Threading.Tasks;

public class LevelFinishedMessage : CanvasLayer
{
    private Label time;
    private Label previousRecord;
    private Label newRecord;
    private MarginContainer container;
    private Button nextLevel;
    private AnimationPlayer animationPlayer;

    private LevelItem nextLevelItem;

    public override void _Ready()
    {
        container = (MarginContainer)GetNode("MarginContainer");
        time = (Label)GetNode("MarginContainer/Panel/CenterContainer/VBoxContainer/TimeWrapper/Value");
        previousRecord = (Label)GetNode("MarginContainer/Panel/CenterContainer/VBoxContainer/PreviousRecordWrapper/Value");
        newRecord = (Label)GetNode("MarginContainer/Panel/CenterContainer/VBoxContainer/NewRecordWrapper/Label");
        nextLevel = (Button)GetNode("MarginContainer/Panel/CenterContainer/VBoxContainer/NextLevelWrapper/Button");
        animationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");

        nextLevelItem = null;
        newRecord.Visible = false;
        container.Visible = false;
    }

    private void OnButtonPressed()
    {
        // go to next level
        if (nextLevelItem != null)
        {
            SharedFunctions.Instance.LoadScene(this, nextLevelItem.ScenePath);
        } else
        {
            SharedFunctions.Instance.LoadScene(this, GlobalValues.MainMenuScene);
        }
    }

    public void Show(LevelItem levelValues, TimeSpan timeSpan)
    {
        nextLevelItem = SharedFunctions.Instance.LevelManager.Next(levelValues.Order);
        if (nextLevelItem == null)
            nextLevel.Text = "Back to Menu";
        else
            nextLevel.Text = "Next Level";

        time.Text = SharedFunctions.Instance.FormatTimeSpan(timeSpan);

        if (levelValues.Record == null || timeSpan < levelValues.Record)
        {
            newRecord.Visible = true;
            animationPlayer.Play("new_record", -1, 5);
        }
        if (levelValues.Record.HasValue)
            previousRecord.Text = SharedFunctions.Instance.FormatTimeSpan(levelValues.Record.Value);
        else
            previousRecord.Text = "--:--:--";

        container.Visible = true;
        Task.Run(() =>
        {
            SharedFunctions.Instance.LevelManager.SaveLevelUserItem(levelValues.Id, timeSpan);
        });
    }
}