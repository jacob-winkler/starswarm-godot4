using Godot;
using StarSwarm.Project.Autoload;
using System;

public class SpaceMineLauncher : WeaponAttachment
{
    [Export]
    public PackedScene SpaceMine { get; set; } = default!;
    public ObjectRegistry ObjectRegistry { get; set; } = default!;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        ObjectRegistry = GetNode<ObjectRegistry>("/root/ObjectRegistry");
        CooldownTimer.Start(Cooldown);
    }

    protected override void FireWeapon()
    {
        var spaceMine = (SpaceMine)SpaceMine.Instance();
        spaceMine.Position = GlobalPosition;
        ObjectRegistry.RegisterProjectiles(spaceMine);
    }
}
