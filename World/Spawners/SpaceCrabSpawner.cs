using Godot;
using System;
namespace StarSwarm.Project.World.Spawners
{
    public class EnemySpawner : Node2D
    {
        [Export]
        public PackedScene SpaceCrab { get; set; } = null!;
        [Export]
        public Int32 CountMin = 1;
        [Export]
        public Int32 CountMax = 5;
        [Export]
        public float SpawnRadius = 150f;

        public void SpawnSpaceCrabs(RandomNumberGenerator rng, Vector2 playerPosition)
        {
            var angle = Mathf.Deg2Rad(rng.RandfRange(1, 360));
            var spawnPosition = new Vector2(
                playerPosition.x + SpawnRadius * Mathf.Cos(angle),
                playerPosition.y + SpawnRadius * Mathf.Sin(angle)
            );
        }
    }
}
