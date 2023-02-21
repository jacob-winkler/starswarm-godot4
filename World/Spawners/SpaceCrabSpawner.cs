using Godot;
using StarSwarm.Project.Ships.Enemies.SpaceCrab;
using System;

namespace StarSwarm.Project.World.Spawners
{
	public class SpaceCrabSpawner : Node2D
	{
		[Export]
		public PackedScene SpaceCrab { get; set; } = default!;
		[Export]
		public Int32 CountMin = 1;
		[Export]
		public Int32 CountMax = 5;
		[Export]
		public float SpawnRadius = 600f;


        public Events Events { get; set; } = default!;
        public CountUpTimer GameTime { get; set; } = default!;
        private RandomNumberGenerator _rng { get; set; } = default!;

		public override void _Ready()
		{
            Events = GetNode<Events>("/root/Events");
            Events.Connect("EnemyAdrift", this, "OnSpaceCrabFellAdrift");
        }

		public void Initialize(RandomNumberGenerator rng)
		{
            _rng = rng;
        }

        public void SpawnSpaceCrabs(Vector2 playerPosition)
		{
			for(var i = 0; i < CountMax; i++)
			{
				var spaceCrab = (SpaceCrab)SpaceCrab.Instance();
                SetSpaceCrabPositionAroundPlayer(spaceCrab, playerPosition);

                AddChild(spaceCrab);
			}
		}

		public void SetSpaceCrabPositionAroundPlayer(SpaceCrab crab, Vector2 playerPosition)
		{
            var angle = Mathf.Deg2Rad(_rng.RandfRange(0, 360));
            var newPosition = playerPosition + new Vector2(
                Mathf.Cos(angle),
                Mathf.Sin(angle)
            ) * SpawnRadius;

            crab.GlobalPosition = newPosition;
            GD.Print("Player global position: "+playerPosition);
            GD.Print("New calculated position: "+newPosition);
            GD.Print("Enemy global position: "+crab.GlobalPosition);
        }

		public void OnSpaceCrabFellAdrift(PhysicsBody2D body, Vector2 playerPosition)
		{
            if (body is SpaceCrab)
            {
                SetSpaceCrabPositionAroundPlayer((SpaceCrab)body, playerPosition);
            }
        }
	}
}
