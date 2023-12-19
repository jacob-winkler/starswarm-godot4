using Godot;
using Main;
using StarSwarm.Autoload;

namespace StarSwarm.World.Spawners;

public partial class RadialSpawner<TNode> : Node2D
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

    private RandomNumberGenerator _rng = new RandomNumberGenerator();
    private int _nodesAlive = 0;

    public override void _Ready()
    {
        _rng.Randomize();
    }

    public void SpawnNodesAroundPosition(Vector2 centralPosition)
    {
        while (_nodesAlive < MaxNodes)
        {
            SpawnNodeAroundPosition(centralPosition);
        }
    }

    public TNode SpawnNodeAroundPosition(Vector2 centralPosition)
    {
        var instantiatedNode = (TNode)NodeToSpawn.Instantiate();
        PlaceSpawnedNodeAroundCentralPosition(instantiatedNode, centralPosition);

        AddChild(instantiatedNode);
        _nodesAlive++;

        return instantiatedNode;
    }

    public void PlaceSpawnedNodeAroundCentralPosition(TNode node, Vector2 centralPosition)
    {
        var angle = Mathf.DegToRad(_rng.RandfRange(0, 360));
        var newPosition = centralPosition + (new Vector2(
            Mathf.Cos(angle),
            Mathf.Sin(angle)
        ) * SpawnRadius);

        node.Position = newPosition - this.GlobalPosition;
    }
}
