using Godot;
using System.Collections.Generic;

public class GameWorld : Node2D
{
        [Export]
    public float Radius = 8000.0f;

    private List<Vector2> _spawnedPositions = new List<Vector2>();
    private List<Node2D> _worldObjects = new List<Node2D>();

    public PlayerSpawner PlayerSpawner { get; set; } = new PlayerSpawner();

    public async override void _Ready()
    {
        await ToSignal(Owner, "ready");
        base._Ready();
        PlayerSpawner = GetNode<PlayerSpawner>("PlayerSpawner");
        Setup();
    }

    public void Setup()
    {
        PlayerSpawner.SpawnPlayer();
    }
}
