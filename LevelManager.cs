using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LevelManager : ISingletonHandler
{
    public List<LevelItem> Levels { get; private set; }
    private List<LevelUserItem> userRecords;

    public LevelManager()
    {
        Levels = new List<LevelItem>();
    }

    public void Init()
    {
        OnLoadLevels();
    }

    private void OnLoadLevels()
    {
        Levels = new List<LevelItem>();
        userRecords = new List<LevelUserItem>();
        userRecords = SharedFunctions.Instance.FileHandler.Load<List<LevelUserItem>>(GlobalValues.UserRecordsPath);

        /*Levels.Add(new LevelItem
        {
            Id = new Guid("6B8C6BC0-6D53-4FC1-8A7B-18131E651802"),
            BronzeTime = TimeSpan.FromMinutes(10),
            SilverTime = TimeSpan.FromMinutes(7),
            GoldTime = TimeSpan.FromMinutes(5),
            ScenePath = "res://LevelTest.tscn",
            Order = 0,
            RequieredQuestions = 1,
            FinishedCount = UserFinishedCountById(userRecords, new Guid("6B8C6BC0-6D53-4FC1-8A7B-18131E651802")),
            Record = UserRecordTimeSpanById(userRecords, new Guid("6B8C6BC0-6D53-4FC1-8A7B-18131E651802")),
            LevelTitle = "Test Level-1",
            LevelDescription = "Internal Test level while Development"
        });*/

        Levels.Add(new LevelItem
        {
            Id = new Guid("85DDBFAB-5208-4F7D-82A9-8FB224CB5403"),
            BronzeTime = TimeSpan.FromMinutes(1),
            SilverTime = TimeSpan.FromSeconds(30),
            GoldTime = TimeSpan.FromSeconds(20),
            ScenePath = "res://levels/Level1.tscn",
            Order = 1,
            RequieredQuestions = 3,
            FinishedCount = UserFinishedCountById(userRecords, new Guid("85DDBFAB-5208-4F7D-82A9-8FB224CB5403")),
            Record = UserRecordTimeSpanById(userRecords, new Guid("85DDBFAB-5208-4F7D-82A9-8FB224CB5403")),
            LevelTitle = "Level 1",
            LevelDescription = "Fast and easy level to start off"
        });
    }

    private TimeSpan? UserRecordTimeSpanById(List<LevelUserItem> userRecords, Guid id)
    {
        if (userRecords != null && userRecords.Any(x => x.Id == id))
        {
            return userRecords.First(x => x.Id == id).Record;
        }
        return null;
    }

    private int UserFinishedCountById(List<LevelUserItem> userRecords, Guid id)
    {
        if (userRecords != null && userRecords.Any(x => x.Id == id))
        {
            return userRecords.First(x => x.Id == id).FinishedCount;
        }
        return 0;
    }

    public LevelItem ById(Guid id)
    {
        return Levels.First(x => x.Id == id);
    }

    public LevelItem Next(int lastOrder)
    {
        var items = Levels.OrderBy(x => x.Order).Where(x => x.Order > lastOrder);
        if (items != null && items.Count() > 0)
        {
            return items.First();
        }
        return null;
    }

    public void SaveLevelUserItem(Guid id, TimeSpan record)
    {
        var level = ById(id);
        level.FinishedCount++;
        if (level.Record == null || record < level.Record)
            level.Record = record;
        if (userRecords == null)
            userRecords = new List<LevelUserItem>();
        if (userRecords.Any(x => x.Id == id))
            userRecords.RemoveAll(x => x.Id == id);
        userRecords.Add(new LevelUserItem
        {
            Id = id,
            FinishedCount = level.FinishedCount,
            Record = level.Record
        });
        SharedFunctions.Instance.FileHandler.Save(userRecords, GlobalValues.UserRecordsPath);
    }

}
