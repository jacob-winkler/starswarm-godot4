using Godot;
using System;

public abstract class WeaponAttachment : Node2D
{
    public Timer CooldownTimer { get; set; } = default!;
    public float Cooldown { get; set; } = 3;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CooldownTimer = GetNode<Timer>("CooldownTimer");
        CooldownTimer.Connect("timeout", this, "OnCooldownExpired");
    }

    // Called every time the cooldown timer expires
    protected abstract void FireWeapon();

    protected void OnCooldownExpired()
    {
        FireWeapon();
    }
}
