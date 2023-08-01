using Godot;
using StarSwarm.Project.Ships.Enemies.SpaceCrab;
using System;

namespace StarSwarm.World.Spawners
{
	public partial class SpaceCrabSpawner : Spawner
	{
		[Export]
		public PackedScene SpaceCrab { get; set; } = default!;
		[Export]
		public Int32 CountMin = 1;
		[Export]
		public Int32 MaxSpaceCrabs = 8;
		[Export]
		public float SpawnRadius = 650f;

        public Events Events { get; set; } = default!;
        public CountUpTimer GameTime { get; set; } = default!;

        private RandomNumberGenerator _rng { get; set; } = default!;
        private Int32 _spaceCrabsAlive { get; set; } = 0;

        public override void _Ready()
		{
            Events = GetNode<Events>("/root/Events");
            Events.Connect("EnemyAdrift", new Callable(this, "OnSpaceCrabFellAdrift"));
            Events.Connect("SpaceCrabDied", new Callable(this, "OnSpaceCrabDied"));
        }

		public void Initialize(PlayerShip playerShip, RandomNumberGenerator rng)
		{
            _playerShip = playerShip;
            _rng = rng;
        }

        public void SpawnSpaceCrabsAroundPlayer() => SpawnSpaceCrabsAroundPosition(_playerShip.GlobalPosition);

        public void SpawnSpaceCrabsAroundPosition(Vector2 position)
		{
			while(_spaceCrabsAlive < MaxSpaceCrabs)
			{
                SpawnSpaceCrab(position);
            }
		}

		public void SpawnSpaceCrab(Vector2 playerPosition)
		{
			var spaceCrab = (SpaceCrab)SpaceCrab.Instantiate();
			SetSpaceCrabPositionAroundPlayer(spaceCrab, playerPosition);

			CallDeferred("add_child", spaceCrab);
			_spaceCrabsAlive++;
		}

		public void SetSpaceCrabPositionAroundPlayer(SpaceCrab crab, Vector2 playerPosition)
		{
            var angle = Mathf.DegToRad(_rng.RandfRange(0, 360));
            var newPosition = playerPosition + (new Vector2(
                Mathf.Cos(angle),
                Mathf.Sin(angle)
            ) * SpawnRadius);

            crab.Position = newPosition - this.GlobalPosition;
        }

		public void OnSpaceCrabFellAdrift(PhysicsBody2D body)
		{
            if (body is SpaceCrab spaceCrab)
            {
                SetSpaceCrabPositionAroundPlayer(spaceCrab, _playerShip.GlobalPosition);
            }
        }

		public void OnSpaceCrabDied()
		{
            _spaceCrabsAlive--;
			if(_spaceCrabsAlive/MaxSpaceCrabs <= .8)
			{
                SpawnSpaceCrabsAroundPosition(_playerShip.GlobalPosition);
            }
        }
	}
}
