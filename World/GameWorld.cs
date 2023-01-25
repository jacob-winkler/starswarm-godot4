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
		public PlayerSpawner PlayerSpawner { get; set; } = null!;
		public SpaceCrabSpawner SpaceCrabSpawner { get; set; } = null!;

		public async override void _Ready()
		{
			await ToSignal(Owner, "ready");
			base._Ready();

			Rng.Randomize();
			PlayerSpawner = GetNode<PlayerSpawner>("PlayerSpawner");
			SpaceCrabSpawner = GetNode<SpaceCrabSpawner>("SpaceCrabSpawner");
			Setup();
		}

		public void Setup()
		{
			var playerPosition = PlayerSpawner.SpawnPlayer();
			SpaceCrabSpawner.SpawnSpaceCrabs(Rng, playerPosition);
		}
	}
}
