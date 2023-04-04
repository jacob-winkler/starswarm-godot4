using Godot;
using System;

public class CountUpTimer : Label
{
    public Events Events { get; set; } = default!;

    /// <summary>
    /// Gets or sets the total number of seconds that have elapsed in the game world.
    /// </summary>
    public float TimeElapsed { get; set; }

    /// <summary>
    /// Tracks the number of minutes that have elapsed.
    /// </summary>
    private Int32 _minute = 0;

    /// <summary>
    /// Tracks the number of seconds that have elapsed.
    /// </summary>
    private Int32 _seconds = 0;

    /// <summary>
    /// The number of points to give to the player as each second passes.
    /// </summary>
    private const Int32 _pointsPerSecond = 150;

    public override void _Ready()
    {
        Events = GetNode<Events>("/root/Events");
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Process(float delta)
    {
        TimeElapsed += delta;
        CalculateTime();
        Text = FormatSeconds();
    }

    private String FormatSeconds()
    {
        var minutes = (Int32)(TimeElapsed / 60);
        var seconds = (Int32)TimeElapsed % 60;

        return String.Format("{0}:{1:00}", minutes, seconds);
    }

    private void CalculateTime()
    {
        var minutes = (Int32)(TimeElapsed / 60);
        if (minutes > _minute)
        {
            _minute = minutes;
            Events.EmitSignal("GameMinutePassed", TimeElapsed);
        }

        var seconds = (Int32)TimeElapsed;
        if (seconds > _seconds)
        {
            _seconds = seconds;
            Events.EmitSignal("AddPoints", _pointsPerSecond);

            if(_seconds % 10 == 0)
                Events.EmitSignal("GameTenSecondsPassed", TimeElapsed);

            if(_seconds % 30 == 0)
                Events.EmitSignal("GameThirtySecondsPassed", TimeElapsed);
        }
    }
}
