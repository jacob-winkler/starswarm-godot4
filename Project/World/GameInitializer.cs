using Godot;
using StarSwarm.Project.Autoload;
using System;

namespace StarSwarm.Project.World
{
	public partial class GameInitializer : Node
	{
		public ObjectRegistry ObjectRegistry { get; set; } = new ObjectRegistry();
		public PlayerCamera Camera3D { get; set; } = new PlayerCamera();
		public Events Events { get; set; } = new Events();

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Events = GetNode<Events>("/root/Events");
			ObjectRegistry = GetNode<ObjectRegistry>("/root/ObjectRegistry");
			Camera3D = GetNode<PlayerCamera>("GameWorld/Camera3D");

			Events.Connect("PlayerSpawned", new Callable(this, "OnPlayerSpawned"));
		}

		public void OnPlayerSpawned(PlayerShip player)
		{
			player.GrabCamera(Camera3D);
			Events.EmitSignal("NodeSpawned", player);
		}
	}
}
