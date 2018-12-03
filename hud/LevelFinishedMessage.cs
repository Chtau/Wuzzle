using Godot;
using System;
using System.Threading.Tasks;

public class LevelFinishedMessage : Control
{
    private Label time;
    private Label previousRecord;
    private Label newRecord;
    private NinePatchRect container;
    private Button nextLevel;
    private Button back;
    private Button retry;
    private AnimationPlayer animationPlayer;
    private TextureRect goldRecord;
    private TextureRect silberRecord;
    private TextureRect bronzeRecord;

    private LevelItem nextLevelItem;
    private LevelItem currentLevelItem;

    public override void _Ready()
    {
        container = (NinePatchRect)GetNode("CanvasLayer/Panel");
        time = (Label)container.GetNode("CenterContainer/VBoxContainer/TimeWrapper/Value");
        previousRecord = (Label)container.GetNode("CenterContainer/VBoxContainer/PreviousRecordWrapper/Value");
        newRecord = (Label)container.GetNode("CenterContainer/VBoxContainer/NewRecordWrapper/Label");
        goldRecord = (TextureRect)container.GetNode("CenterContainer/VBoxContainer/NewRecordWrapper/GoldRecord");
        silberRecord = (TextureRect)container.GetNode("CenterContainer/VBoxContainer/NewRecordWrapper/SilberRecord");
        bronzeRecord = (TextureRect)container.GetNode("CenterContainer/VBoxContainer/NewRecordWrapper/BronzeRecord");
        nextLevel = (Button)container.GetNode("CenterContainer/VBoxContainer/NextLevelWrapper/NextLevel");
        back = (Button)container.GetNode("CenterContainer/VBoxContainer/NextLevelWrapper/BackToMenu");
        retry = (Button)container.GetNode("CenterContainer/VBoxContainer/NextLevelWrapper/Retry");
        animationPlayer = (AnimationPlayer)GetNode("AnimationPlayer");

        goldRecord.Visible = false;
        silberRecord.Visible = false;
        bronzeRecord.Visible = false;
        this.Visible = false;
        nextLevelItem = null;
        newRecord.Visible = false;
        container.Visible = false;
    }

    private void OnButtonPressed()
    {
        SharedFunctions.Instance.LoadScene(this, nextLevelItem.ScenePath);
    }

    private void OnBackPressed()
    {
        SharedFunctions.Instance.LoadScene(this, GlobalValues.MainMenuScene);
    }

    private void OnRetryPressed()
    {
        SharedFunctions.Instance.LoadScene(this, currentLevelItem.ScenePath);
    }

    public void Show(LevelItem levelValues, TimeSpan timeSpan)
    {
        currentLevelItem = levelValues;
        nextLevelItem = SharedFunctions.Instance.LevelManager.Next(levelValues.Order);
        if (nextLevelItem == null)
        {
            nextLevel.Visible = false;
        }
        else
        {
            nextLevel.Visible = true;
        }

        time.Text = SharedFunctions.Instance.FormatTimeSpan(timeSpan);

        if (levelValues.Record == null || timeSpan < levelValues.Record)
        {
            newRecord.Visible = true;
            animationPlayer.Play("new_record", -1, 5);
            if (timeSpan <= levelValues.GoldTime)
            {
                goldRecord.Visible = true;
            } else if (timeSpan <= levelValues.SilverTime)
            {
                silberRecord.Visible = true;
            } else if (timeSpan <= levelValues.BronzeTime)
            {
                bronzeRecord.Visible = true;
            }
        }
        if (levelValues.Record.HasValue)
            previousRecord.Text = SharedFunctions.Instance.FormatTimeSpan(levelValues.Record.Value);
        else
            previousRecord.Text = "--:--:--";

        this.Visible = true;
        container.Visible = true;
        Task.Run(() =>
        {
            SharedFunctions.Instance.LevelManager.SaveLevelUserItem(levelValues.Id, timeSpan);
        });
    }

    public void SetFocus()
    {
        if (nextLevel.Visible)
        {
            nextLevel.GrabFocus();
        } else
        {
            retry.GrabFocus();
        }
    }
}