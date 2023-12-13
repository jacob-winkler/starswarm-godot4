using Godot;
using Main;
using StarSwarm.Autoload;
using StarSwarm.Ships.Player;

namespace StarSwarm.World.Spawners;

public partial class RadialSpawner<TNode> : Spawner
    where TNode : Node2D
{
    [Export]
    public PackedScene NodeToSpawn { get; set; } = default!;
    [Export]
    public int CountMin = 1;
    [Export]
    public int MaxNodes = 8;
    [Export]
    public float SpawnRadius = 650f;

    public Events Events { get; set; } = default!;
    public CountUpTimer GameTime { get; set; } = default!;

    private RandomNumberGenerator _rng = default!;
    private int _nodesAlive = 0;

    public override void _Ready()
    { }

    public void Initialize(PlayerShip playerShip, RandomNumberGenerator rng)
    {
        _playerShip = playerShip;
        _rng = rng;
    }

    public void SpawnNodesAroundPosition(Vector2 position)
    {
        while (_nodesAlive < MaxNodes)
        {
            SpawnNode(position);
        }
    }

    public void SpawnNode(Vector2 centralPoint)
    {
        var instantiatedNode = (TNode)NodeToSpawn.Instantiate();
        PositionSpawnedNodeAroundCentralPosition(instantiatedNode, centralPoint);

        CallDeferred("add_child", instantiatedNode);
        _nodesAlive++;
    }

    public void PositionSpawnedNodeAroundCentralPosition(TNode node, Vector2 centralPoint)
    {
        var angle = Mathf.DegToRad(_rng.RandfRange(0, 360));
        var newPosition = centralPoint + (new Vector2(
            Mathf.Cos(angle),
            Mathf.Sin(angle)
        ) * SpawnRadius);

        node.Position = newPosition - this.GlobalPosition;
    }
}
