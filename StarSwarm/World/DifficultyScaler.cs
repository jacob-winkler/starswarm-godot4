using StarSwarm.Autoload;
using Godot;
using StarSwarm.World.Spawners;

namespace StarSwarm.World;

public partial class DifficultyScaler : Node
{
    public Events Events { get; set; } = default!;
    public SpaceCrabSpawner SpaceCrabSpawner { get; set; } = default!;

    public override void _Ready()
    {
        SpaceCrabSpawner = GetNode<SpaceCrabSpawner>("../SpaceCrabSpawner");
        Events = GetNode<Events>("/root/Events");
        Events.Connect("GameTenSecondsPassed", new Callable(this, "OnTenSecondsPassed"));
    }

    public void OnTenSecondsPassed(float totalTimeElapsed)
    {
        SpaceCrabSpawner.MaxSpaceCrabs += 3;
        SpaceCrabSpawner.SpawnSpaceCrabsAroundPlayer();
    }
}