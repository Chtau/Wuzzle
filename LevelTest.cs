using Godot;
using System;

public class LevelTest : Node, ILevelConfiguration
{
    public int RequieredQuestions => 7;

    public TimeSpan GoldTime => TimeSpan.FromMinutes(5);

    public TimeSpan SilverTime => TimeSpan.FromMinutes(7);

    public TimeSpan BronzeTime => TimeSpan.FromMinutes(10);
}
