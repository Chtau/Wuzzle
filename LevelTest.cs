using Godot;
using System;

public class LevelTest : Node, ILevelConfiguration
{
    public int RequieredQuestions => 7;

    public TimeSpan TimeLimit => TimeSpan.FromMinutes(30);
}
