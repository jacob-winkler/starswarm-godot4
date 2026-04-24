using Godot;

namespace StarSwarm.World.Spawners;
public partial class SpawnPoint : Node2D
{
    public override void _Ready()
    {
        var sprite = GetNode<Sprite2D>("Sprite2D");
        sprite.Scale = new Vector2(2, 2);

        var tween = CreateTween();
        tween.SetLoops();
        tween.TweenProperty(sprite, "rotation", -Mathf.Tau, 3.0f)
            .AsRelative();
    }
}
