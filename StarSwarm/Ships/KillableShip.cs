using Godot;
using StarSwarm.GSAI_Framework;
using StarSwarm.Infrastructure;
using StarSwarm.Weapons;

namespace StarSwarm.Ships;
public abstract partial class KillableShip : GSAICharacterBody2D, IKillable
{
    [Export]
    public float HealthMax = 100f;
    [Export]
    public float LinearSpeedMax = 200f;
    [Export]
    public float AccelerationMax = 300f;
    [Export]
    public float DragFactor = 0.04f;
    [Export]
    public float AngularSpeedMax = 200;
    [Export]
    public float AngularAccelerationMax = 3600f;
    [Export]
    public float AngularDragFactor = 0.05f;

    protected float _health;

    public abstract void TakeDamage(float damage, DamageType type);
}
