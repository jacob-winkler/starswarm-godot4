using Godot;
using System;

public class CountUpTimer : Label
{
    public Events Events { get; set; } = default!;

    /// <summary>
    /// Gets or sets the total number of seconds that have elapsed in the game world.
    /// </summary>
    public float TimeElapsed { get; set; } = 55f;

    /// <summary>
    /// Tracks the number of minutes that have elapsed.
    /// Used to send a signal each time the elapsed minutes increases.
    /// </summary>
    private Int32 _minute = 0;

    public override void _Ready()
    {
        Events = GetNode<Events>("/root/Events");
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Process(float delta)
    {
        TimeElapsed += delta;
        Text = FormatSeconds(TimeElapsed);
    }

    private String FormatSeconds(float time)
    {
        var minutes = (Int32)(time / 60);
        if (minutes > _minute)
        {
            _minute = minutes;
            Events.EmitSignal("GameMinutePassed");
        }

        var seconds = time % 60;

        return String.Format("{0}:{1:00}", minutes, seconds);
    }
}
