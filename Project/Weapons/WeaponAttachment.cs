using Godot;
using System;

public abstract class WeaponAttachment : Node2D
{
    protected Timer _cooldownTimer { get; set; } = default!;

    /// <summary>
    /// A timer for tracking the duration that the weapon should fire for.
    /// If you use this timer, be sure to implement <see cref="StopFiringWeapon"/>.
    /// </summary>
    protected Timer _durationTimer { get; set; } = default!;

    [Export]
    public float Cooldown { get; set; } = 3;
    [Export]
    public float WeaponDuration { get; set; } = 2;
    [Export]
    public Texture SmallIcon { get; set; } = default!;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _cooldownTimer = GetNode<Timer>("CooldownTimer");
        _cooldownTimer.Connect("timeout", this, "OnCooldownExpired");
        _durationTimer = GetNode<Timer>("DurationTimer");
        _durationTimer.Connect("timeout", this, "OnDurationUp");
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
