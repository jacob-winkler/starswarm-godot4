using Godot;
using StarSwarm.Project.Main;
using StarSwarm.World.Spawners;
using System;

namespace StarSwarm.World;

public partial class GameWorld : Node2D
{
    [Export]
    public float Radius = 8000.0f;

    public RandomNumberGenerator Rng { get; set; } = new RandomNumberGenerator();
    public PlayerSpawner PlayerSpawner { get; set; } = default!;
    public SpaceCrabSpawner SpaceCrabSpawner { get; set; } = default!;
    public PlanetSpawner PlanetSpawner { get; set; } = default!;
    public PlayerShip Player { get; set; } = default!;
    public HealthBarUpdater HealthBarUpdater { get; set; } = default!;
    public GameOver GameOverScreen { get; set; } = default!;

    private Boolean _playerDead;

    public override async void _Ready()
    {
        await ToSignal(Owner, "ready");
        base._Ready();

        Rng.Randomize();

        PlayerSpawner = GetNode<PlayerSpawner>("PlayerSpawner");
        Player = GetNode<PlayerShip>("PlayerSpawner/PlayerShip");
        SpaceCrabSpawner = GetNode<SpaceCrabSpawner>("SpaceCrabSpawner");
        PlanetSpawner = GetNode<PlanetSpawner>("PlanetSpawner");
        HealthBarUpdater = GetNode<HealthBarUpdater>("HealthBarUpdater");
        GameOverScreen = GetNode<GameOver>("UI/GameOver");

        Setup();
    }

    public void Setup()
    {
        Player.Connect("Died", new Callable(this, "OnPlayerDied"));

        PlayerSpawner.SpawnPlayer();

        SpaceCrabSpawner.Initialize(Player, Rng);
        SpaceCrabSpawner.SpawnSpaceCrabsAroundPlayer();

        PlanetSpawner.Initialize(Player, Rng);
        PlanetSpawner.SpawnInitialPlanets();

        HealthBarUpdater.Initialize(Player);
        GameOverScreen.ProcessMode = ProcessModeEnum.Always;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_playerDead)
            UpdateHealthBarPosition();
    }

    private void UpdateHealthBarPosition()
    {
        var healthBarPosition = Player.GlobalPosition;
        healthBarPosition.X -= 16;
        healthBarPosition.Y += 24;
        HealthBarUpdater.SetGlobalPosition(healthBarPosition);
    }

    private void OnPlayerDied()
    {
        _playerDead = true;
        GameOverScreen.GlobalPosition = Player.GlobalPosition;
        GameOverScreen.Start();

        GetTree().Paused = true;
    }
}