using Godot;
using System;

namespace StarSwarm.Project.Weapons.LaserBeam;

public partial class LaserBeamAttachment : WeaponAttachment
{
    public LaserBeam LaserBeam { get; set; } = default!;

    public override void _Ready()
    {
        base._Ready();
        _cooldownTimer.Start(Cooldown);
        LaserBeam = GetNode<LaserBeam>("LaserBeam");
    }

    protected override void FireWeapon()
    {
        _durationTimer.Stop();
        LaserBeam.IsCasting = true;
        _durationTimer.Start(WeaponDuration);
    }

    protected override void StopFiringWeapon()
    {
        LaserBeam.IsCasting = false;
    }
}