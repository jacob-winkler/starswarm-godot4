using System;
using Godot;

namespace StarSwarm.Weapons;

public abstract partial class WeaponAttachment : Node2D
{
    protected Timer CooldownTimer { get; set; } = default!;

    /// <summary>
    /// A timer for tracking the duration that the weapon should fire for.
    /// If you use this timer, be sure to implement <see cref="StopFiringWeapon"/>.
    /// </summary>
    protected Timer DurationTimer { get; set; } = default!;

    [Export]
    public float Cooldown { get; set; } = 3;
    [Export]
    public float WeaponDuration { get; set; } = 2;
    [Export]
    public Texture2D SmallIcon { get; set; } = default!;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CooldownTimer = GetNode<Timer>("CooldownTimer");
        CooldownTimer.Connect("timeout", new Callable(this, "OnCooldownExpired"));
        DurationTimer = GetNode<Timer>("DurationTimer");
        DurationTimer.Connect("timeout", new Callable(this, "OnDurationUp"));
    }

    /// <summary>
    /// Called every time the cooldown timer expires
    /// </summary>
    protected abstract void FireWeapon();

    /// <summary>
    /// Called if the duration timer expires. You have to start the duration timer manually.
    /// </summary>
    protected virtual void StopFiringWeapon()
    {
        throw new NotImplementedException();
    }

    private void OnCooldownExpired()
    {
        FireWeapon();
    }

    private void OnDurationUp()
    {
        StopFiringWeapon();
    }
}
