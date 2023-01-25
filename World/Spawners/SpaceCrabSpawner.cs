using Godot;
using StarSwarm.Project.Ships.Enemies.SpaceCrab;
using System;

namespace StarSwarm.Project.World.Spawners
{
	public class SpaceCrabSpawner : Node2D
	{
		[Export]
		public PackedScene SpaceCrab { get; set; } = null!;
		[Export]
		public Int32 CountMin = 1;
		[Export]
		public Int32 CountMax = 5;
		[Export]
		public float SpawnRadius = 5f;

		public void SpawnSpaceCrabs(RandomNumberGenerator rng, Vector2 playerPosition)
		{
			for(var i = 0; i < CountMax; i++)
			{
				var spaceCrab = (SpaceCrab)SpaceCrab.Instance();

				var angle = Mathf.Deg2Rad(rng.RandfRange(1, 360));
				var spawnPosition = new Vector2(
					playerPosition.x + SpawnRadius * Mathf.Cos(angle),
					playerPosition.y + SpawnRadius * Mathf.Sin(angle)
				);

				spaceCrab.Position = spawnPosition;
			}
		}
	}
}
