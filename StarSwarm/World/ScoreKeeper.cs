using StarSwarm.Autoload;
using Godot;

namespace World;

public partial class ScoreKeeper : Label
{
    public int TotalScore { get; set; } = 0;
    public Events Events { get; set; } = default!;

    public override void _Ready()
    {
        Events = GetNode<Events>("/root/Events");
        Events.Connect("AddPoints", new Callable(this, "OnAddPoints"));
    }

    public override void _Process(double delta)
    {
        Text = TotalScore.ToString();
    }

    public void OnAddPoints(int points)
    {
        TotalScore += points;
    }
}
