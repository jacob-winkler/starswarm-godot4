using Godot;
using StarSwarm.Planets;
using StarSwarm.StarSwarm.World.Spawners;

namespace StarSwarm.World.Spawners;

public partial class SwarmWaveSpawner : Node2D
{
    public Timer WaveTimer { get; set; } = default!;
    public Timer SpawnTimer { get; set; } = default!;
    public SentientGooSpawner SentientGooSpawner { get; set; } = default!;

    private Planet _planet = default!;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        SpawnTimer = GetNode<Timer>("SpawnTimer");
        SentientGooSpawner = GetNode<SentientGooSpawner>("SentientGooSpawner");
	}

    public void Initialize(Planet planet)
    {
        _planet = planet;
    }

	public void StartSwarmWave(int spawnTime)
    {
        SpawnTimer.Connect("timeout", new Callable(this, "SpawnSwarmEnemy"));
        SpawnTimer.Start(spawnTime);
    }

    public void SpawnSwarmEnemy()
    {
        var spawnedEnemy = SentientGooSpawner.SpawnNodeAroundPosition(_planet.GlobalPosition);
    }

    public void StopSwarmWave()
    {
        SpawnTimer.Stop();
    }
}
