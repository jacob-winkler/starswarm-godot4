using Godot;
using StarSwarm.Ships.Player;

namespace StarSwarm.World.Spawners;

public abstract partial class Spawner : Node2D
{
    protected PlayerShip _playerShip = default!;
}