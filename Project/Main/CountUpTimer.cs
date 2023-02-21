using Godot;
using System;

public class CountUpTimer : Label
{
    /// <summary>
    /// Gets or sets the total number of seconds that have elapsed in the game world.
    /// </summary>
    public float TimeElapsed { get; set; } = 0.0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Process(float delta)
    {
        TimeElapsed += delta;
        Text = FormatSeconds(TimeElapsed);
    }

    private String FormatSeconds(float time)
    {
        var minutes = (Int32)(time / 60);
        var seconds = time % 60;

        return String.Format("{0}:{1:00}", minutes, seconds);
    }
}
