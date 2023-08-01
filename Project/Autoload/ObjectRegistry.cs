using Godot;

namespace StarSwarm.Project.Autoload;

/// <summary>
/// Creates, maintains, and organizes spawned special effects or projectiles;
/// objects that should be untied from their spawners' lifespan when freed.
/// </summary>
public partial class ObjectRegistry : Node
{
    private Node2D _projectiles = new();
    private Node2D _effects = new();
    private SubViewport _distortions = new();

    public override void _Ready()
    {
        _projectiles = GetNode<Node2D>("Projectiles");
        _effects = GetNode<Node2D>("Effects");
    }

    public void RegisterEffect(Node effect) => _effects.AddChild(effect);

    public void RegisterProjectiles(Node projectile) => _projectiles.AddChild(projectile);

    public void RegisterDistortionEffect(Node2D effect) => _distortions?.AddChild(effect);

    public void RegisterDistortionParent(SubViewport viewport) => _distortions = viewport;
}