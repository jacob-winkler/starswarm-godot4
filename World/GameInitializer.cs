using Godot;
using StarSwarm.Project.Autoload;
using System;

namespace StarSwarm.Project.World
{
    public class GameInitializer : Node
    {
        public ObjectRegistry ObjectRegistry { get; set; } = new ObjectRegistry();
        public PlayerCamera Camera { get; set; } = new PlayerCamera();
        public Events Events { get; set; } = new Events();

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            Events = GetNode<Events>("/root/Events");
            ObjectRegistry = GetNode<ObjectRegistry>("/root/ObjectRegistry");
            Camera = GetNode<PlayerCamera>("GameWorld/Camera");

            Events.Connect("PlayerSpawned", this, "OnPlayerSpawned");

            //ObjectRegistry.RegisterDistortionParent(GetNode<Viewport>("DistortMaskView/Viewport"));
            Camera.SetupDistortionCamera();
        }

        public void OnPlayerSpawned(PlayerShip player)
        {
            player.GrabCamera(Camera);
            Events.EmitSignal("NodeSpawned", player);
        }
    }
}
