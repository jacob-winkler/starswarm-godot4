using Godot;
using StarSwarm.Project.Autoload;
using System;

public partial class SpaceMineAttachment : WeaponAttachment
{
    [Export]
    public PackedScene SpaceMine { get; set; } = default!;
    public ObjectRegistry ObjectRegistry { get; set; } = default!;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        ObjectRegistry = GetNode<ObjectRegistry>("/root/ObjectRegistry");
        _cooldownTimer.Start(Cooldown);
    }

    protected override void FireWeapon()
    {
        var spaceMine = (SpaceMine)SpaceMine.Instantiate();
        spaceMine.Position = GlobalPosition;
        ObjectRegistry.RegisterProjectiles(spaceMine);
    }
}
