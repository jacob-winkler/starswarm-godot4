using Godot;
using System;

public class ScoreKeeper : Label
{
    public Int32 TotalScore { get; set; } = 0;
    public Events Events { get; set; } = default!;

    public override void _Ready()
    {
        Events = GetNode<Events>("/root/Events");
        Events.Connect("AddPoints", this, "OnAddPoints");
    }

    public override void _Process(float delta)
    {
        Text = TotalScore.ToString();
    }

    public void OnAddPoints(Int32 points)
    {
        TotalScore += points;
    }
}
