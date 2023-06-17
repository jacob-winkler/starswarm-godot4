using Godot;
using System;

public partial class ResearchBar : ProgressBar
{
    [Signal]
    public delegate void ResearchFinishedEventHandler();
    private Boolean _researching = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Hide();
    }

    public void BeginResearch(float researchTime)
    {
        MaxValue = researchTime;
        Value = 0;
        Show();
        _researching = true;
    }

    public override void _Process(double delta)
    {
        if (_researching)
        {
            Value += delta;

            if (Value >= MaxValue)
                FinishResearch();
        }
    }

    private void FinishResearch()
    {
        Hide();
        _researching = false;
        EmitSignal("ResearchFinished");
    }
}
