using Godot;

namespace StarSwarm.World.Spawners;

public abstract partial class Spawner : Node2D
{
    protected PlayerShip _playerShip = default!;
}