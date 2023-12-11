namespace StarSwarm.Weapons.LaserBeam;

public partial class LaserBeamAttachment : WeaponAttachment
{
    public LaserBeam LaserBeam { get; set; } = default!;

    public override void _Ready()
    {
        base._Ready();
        CooldownTimer.Start(Cooldown);
        LaserBeam = GetNode<LaserBeam>("LaserBeam");
    }

    protected override void FireWeapon()
    {
        DurationTimer.Stop();
        LaserBeam.IsCasting = true;
        DurationTimer.Start(WeaponDuration);
    }

    protected override void StopFiringWeapon()
    {
        LaserBeam.IsCasting = false;
    }
}