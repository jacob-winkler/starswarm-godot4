using Godot;

namespace StarSwarm.World.Spawners
{
    public partial class PlayerSpawner : Node2D
	{
		public PlayerShip PlayerShip { get; set; } = new PlayerShip();
		public Events Events { get; set; } = new Events();

		public override void _Ready()
		{
			Events = GetNode<Events>("/root/Events");
			PlayerShip = GetNode<PlayerShip>("PlayerShip");
		}

		public Vector2 SpawnPlayer()
		{ 
			Events.EmitSignal("PlayerSpawned", PlayerShip);
			return PlayerShip.GlobalPosition;
		}
	}
}
