using Godot;
using StarSwarm.Autoload;

namespace Main;

public partial class CountUpTimer : Label
{
    public Events Events { get; set; } = default!;

    /// <summary>
    /// Gets or sets the total number of seconds that have elapsed in the game world.
    /// </summary>
    public double TimeElapsed { get; set; }

    /// <summary>
    /// Tracks the number of minutes that have elapsed.
    /// </summary>
    private int _minute = 0;

    /// <summary>
    /// Tracks the number of seconds that have elapsed.
    /// </summary>
    private int _seconds = 0;

    /// <summary>
    /// The number of points to give to the player as each second passes.
    /// </summary>
    private const int _pointsPerSecond = 150;

    public override void _Ready()
    {
        Events = GetNode<Events>("/root/Events");
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Process(double delta)
    {
        TimeElapsed += delta;
        CalculateTime();
        Text = FormatSeconds();
    }

    private string FormatSeconds()
    {
        var minutes = (int)(TimeElapsed / 60);
        var seconds = (int)TimeElapsed % 60;

        return string.Format("{0}:{1:00}", minutes, seconds);
    }

    private void CalculateTime()
    {
        var minutes = (int)(TimeElapsed / 60);
        if (minutes > _minute)
        {
            _minute = minutes;
            Events.EmitSignal("GameMinutePassed", TimeElapsed);
        }

        var seconds = (int)TimeElapsed;
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
