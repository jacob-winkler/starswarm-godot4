using Godot;
using StarSwarm.Project.Autoload;
using System;

public class PlayerSpawner : Node2D
{
    public PlayerShip PlayerShip { get; set; } = new PlayerShip();
    public Events Events { get; set; } = new Events();

    public override void _Ready()
    {
        Events = GetNode<Events>("/root/Events");
        PlayerShip = GetNode<PlayerShip>("PlayerShip");
    }

    public void SpawnPlayer()
    { 
        Events.EmitSignal("PlayerSpawned", PlayerShip);
    }
}
