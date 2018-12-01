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
            TotalQuestionAvailable = 6,
            FinishedCount = UserFinishedCountById(userRecords, new Guid("85DDBFAB-5208-4F7D-82A9-8FB224CB5403")),
            Record = UserRecordTimeSpanById(userRecords, new Guid("85DDBFAB-5208-4F7D-82A9-8FB224CB5403")),
            LevelTitle = "Level 1",
            LevelDescription = "Fast and easy level to start off"
        });
        Levels.Add(new LevelItem
        {
            Id = new Guid("B71143C0-09AE-43EF-8033-7E5AC330478D"),
            BronzeTime = TimeSpan.FromMinutes(2),
            SilverTime = TimeSpan.FromMinutes(1),
            GoldTime = TimeSpan.FromSeconds(40),
            ScenePath = "res://levels/Level2.tscn",
            Order = 2,
            RequieredQuestions = 4,
            TotalQuestionAvailable = 8,
            FinishedCount = UserFinishedCountById(userRecords, new Guid("B71143C0-09AE-43EF-8033-7E5AC330478D")),
            Record = UserRecordTimeSpanById(userRecords, new Guid("B71143C0-09AE-43EF-8033-7E5AC330478D")),
            LevelTitle = "Level 2",
            LevelDescription = "Get going with a little more"
        });
        Levels.Add(new LevelItem
        {
            Id = new Guid("C2FB64C8-7201-48D8-9129-5149AB16031D"),
            BronzeTime = TimeSpan.FromMinutes(3),
            SilverTime = TimeSpan.FromMinutes(2),
            GoldTime = TimeSpan.FromMinutes(1),
            ScenePath = "res://levels/Level3.tscn",
            Order = 3,
            RequieredQuestions = 5,
            TotalQuestionAvailable = 7,
            FinishedCount = UserFinishedCountById(userRecords, new Guid("C2FB64C8-7201-48D8-9129-5149AB16031D")),
            Record = UserRecordTimeSpanById(userRecords, new Guid("C2FB64C8-7201-48D8-9129-5149AB16031D")),
            LevelTitle = "Level 3",
            LevelDescription = "Better get the Questions right"
        });
        Levels.Add(new LevelItem
        {
            Id = new Guid("E4F88F72-4344-4BDB-947C-DDAEAAA2112C"),
            BronzeTime = TimeSpan.FromMinutes(1),
            SilverTime = TimeSpan.FromSeconds(40),
            GoldTime = TimeSpan.FromSeconds(30),
            ScenePath = "res://levels/Level4.tscn",
            Order = 4,
            RequieredQuestions = 3,
            TotalQuestionAvailable = 10,
            FinishedCount = UserFinishedCountById(userRecords, new Guid("E4F88F72-4344-4BDB-947C-DDAEAAA2112C")),
            Record = UserRecordTimeSpanById(userRecords, new Guid("E4F88F72-4344-4BDB-947C-DDAEAAA2112C")),
            LevelTitle = "Level 4",
            LevelDescription = "Don't fall down"
        });
        Levels.Add(new LevelItem
        {
            Id = new Guid("1C1FD23E-7C01-4478-8311-386A3BFBCEB2"),
            BronzeTime = TimeSpan.FromMinutes(1),
            SilverTime = TimeSpan.FromSeconds(40),
            GoldTime = TimeSpan.FromSeconds(30),
            ScenePath = "res://levels/Level5.tscn",
            Order = 5,
            RequieredQuestions = 4,
            TotalQuestionAvailable = 7,
            FinishedCount = UserFinishedCountById(userRecords, new Guid("1C1FD23E-7C01-4478-8311-386A3BFBCEB2")),
            Record = UserRecordTimeSpanById(userRecords, new Guid("1C1FD23E-7C01-4478-8311-386A3BFBCEB2")),
            LevelTitle = "Level 5",
            LevelDescription = "Fall of trust"
        });
        Levels.Add(new LevelItem
        {
            Id = new Guid("1B398B84-F303-4251-9CFE-F4A0B40EC44D"),
            BronzeTime = TimeSpan.FromMinutes(3),
            SilverTime = TimeSpan.FromMinutes(2),
            GoldTime = TimeSpan.FromMinutes(1),
            ScenePath = "res://levels/Level6.tscn",
            Order = 6,
            RequieredQuestions = 9,
            TotalQuestionAvailable = 10,
            FinishedCount = UserFinishedCountById(userRecords, new Guid("1B398B84-F303-4251-9CFE-F4A0B40EC44D")),
            Record = UserRecordTimeSpanById(userRecords, new Guid("1B398B84-F303-4251-9CFE-F4A0B40EC44D")),
            LevelTitle = "Level 6",
            LevelDescription = "Only one wrong answer"
        });
        Levels.Add(new LevelItem
        {
            Id = new Guid("80CEDDF9-C10C-4137-B437-99DFBB55930B"),
            BronzeTime = TimeSpan.FromMinutes(30),
            SilverTime = TimeSpan.FromMinutes(25),
            GoldTime = TimeSpan.FromSeconds(15),
            ScenePath = "res://levels/Level7.tscn",
            Order = 7,
            RequieredQuestions = 1,
            TotalQuestionAvailable = 3,
            FinishedCount = UserFinishedCountById(userRecords, new Guid("80CEDDF9-C10C-4137-B437-99DFBB55930B")),
            Record = UserRecordTimeSpanById(userRecords, new Guid("80CEDDF9-C10C-4137-B437-99DFBB55930B")),
            LevelTitle = "Level 7",
            LevelDescription = "Survive and go fast"
        });
        Levels.Add(new LevelItem
        {
            Id = new Guid("8F732AC0-6178-4680-AAEB-59B6B1A93762"),
            BronzeTime = TimeSpan.FromMinutes(20),
            SilverTime = TimeSpan.FromMinutes(10),
            GoldTime = TimeSpan.FromSeconds(5),
            ScenePath = "res://levels/Level8.tscn",
            Order = 8,
            RequieredQuestions = 1,
            TotalQuestionAvailable = 1,
            FinishedCount = UserFinishedCountById(userRecords, new Guid("8F732AC0-6178-4680-AAEB-59B6B1A93762")),
            Record = UserRecordTimeSpanById(userRecords, new Guid("8F732AC0-6178-4680-AAEB-59B6B1A93762")),
            LevelTitle = "Level 8",
            LevelDescription = "Leap of faith"
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
