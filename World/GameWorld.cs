using Godot;
using StarSwarm.Project.World.Spawners;
using System.Collections.Generic;

namespace StarSwarm.Project.World
{
	public class GameWorld : Node2D
	{
			[Export]
		public float Radius = 8000.0f;

		private List<Vector2> _spawnedPositions = new List<Vector2>();
		private List<Node2D> _worldObjects = new List<Node2D>();

		public RandomNumberGenerator Rng { get; set; } = new RandomNumberGenerator();
		public PlayerSpawner PlayerSpawner { get; set; } = default!;
		public SpaceCrabSpawner SpaceCrabSpawner { get; set; } = default!;
		public PlayerShip Player { get; set; } = default!;
		public HealthBarUpdater HealthBarUpdater { get; set; }= default!;

		public async override void _Ready()
		{
			await ToSignal(Owner, "ready");
			base._Ready();

			Rng.Randomize();
			PlayerSpawner = GetNode<PlayerSpawner>("PlayerSpawner");
			SpaceCrabSpawner = GetNode<SpaceCrabSpawner>("SpaceCrabSpawner");
            SpaceCrabSpawner.Initialize(Rng);
            Player = GetNode<PlayerShip>("PlayerSpawner/PlayerShip");
			HealthBarUpdater = GetNode<HealthBarUpdater>("HealthBarUpdater");
			Setup();
		}

		public void Setup()
		{
			var playerPosition = PlayerSpawner.SpawnPlayer();
			SpaceCrabSpawner.SpawnSpaceCrabs(playerPosition);
			HealthBarUpdater.Initialize(Player);
		}

		public override void _PhysicsProcess(float delta)
		{
			var healthBarPosition = Player.GlobalPosition;
			healthBarPosition.x -= 16;
			healthBarPosition.y += 24;
			HealthBarUpdater.SetGlobalPosition(healthBarPosition);
		}
	}
}
