using StarSwarm.Autoload;
using Godot;
using StarSwarm.Autoload;
using StarSwarm.Ships.Player;

namespace StarSwarm.World;

public partial class GameInitializer : Node
	{
		public ObjectRegistry ObjectRegistry { get; set; } = new ObjectRegistry();
		public PlayerCamera Camera2D { get; set; } = new PlayerCamera();
		public Events Events { get; set; } = new Events();

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Events = GetNode<Events>("/root/Events");
			ObjectRegistry = GetNode<ObjectRegistry>("/root/ObjectRegistry");
        Camera2D = GetNode<PlayerCamera>("GameWorld/Camera2D");

			Events.Connect("PlayerSpawned", new Callable(this, "OnPlayerSpawned"));
		}

		public void OnPlayerSpawned(PlayerShip player)
		{
			player.GrabCamera(Camera2D);
			Events.EmitSignal("NodeSpawned", player);
		}
	}
