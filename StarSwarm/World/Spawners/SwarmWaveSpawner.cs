using System.Collections.Generic;
using Godot;
using StarSwarm.Planets;
using StarSwarm.StarSwarm.World.Spawners;

namespace StarSwarm.World.Spawners;

public partial class SwarmWaveSpawner : Node2D
{
    [Export]
    public float SpawnRadius = 650f;

    [Export]
    public PackedScene SpawnPointScene { get; set; } = default!;

    public Timer WaveTimer { get; set; } = default!;
    public Timer SpawnTimer { get; set; } = default!;
    public SentientGooSpawner SentientGooSpawner { get; set; } = default!;

    private Planet _planet = default!;

    private List<SpawnPoint> _spawnPoints = new List<SpawnPoint>();
    private RandomNumberGenerator _rng = new RandomNumberGenerator();

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
        CreateSpawnPoints(3);
        SpawnTimer.Connect("timeout", new Callable(this, "SpawnSwarmEnemy"));
        SpawnTimer.Start(spawnTime);
    }

    private void CreateSpawnPoints(int number)
    {
        for (var i = 0; i < number; i++)
        {
            var angle = Mathf.DegToRad(_rng.RandfRange(0, 360));
            var newPosition = _planet.GlobalPosition + (new Vector2(
                Mathf.Cos(angle),
                Mathf.Sin(angle)
            ) * SpawnRadius);

            var spawnPoint = SpawnPointScene.Instantiate<SpawnPoint>();
            spawnPoint.Position = newPosition - this.GlobalPosition;
            AddChild(spawnPoint);
            _spawnPoints.Add(spawnPoint);
        }
    }

    public void SpawnSwarmEnemy()
    {
        var spawnPoint = _spawnPoints[_rng.RandiRange(0, _spawnPoints.Count - 1)];
        var spawnedEnemy = SentientGooSpawner.SpawnAtPosition(spawnPoint.Position);
        spawnedEnemy.Attack(_planet);
    }

    public void StopSwarmWave()
    {
        SpawnTimer.Stop();
        foreach (var spawnPoint in _spawnPoints)
            spawnPoint.QueueFree();
        _spawnPoints.Clear();
    }
}
