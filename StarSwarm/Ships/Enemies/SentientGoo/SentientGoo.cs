using System;
using System.Collections.Generic;
using Godot;
using StarSwarm.Autoload;
using StarSwarm.Planets;
using StarSwarm.StarSwarm.Ships;
using StarSwarm.SWStateMachine;
using StarSwarm.VFX;
using StarSwarm.Weapons;

namespace StarSwarm.Ships.Enemies.SentientGoo;

public partial class SentientGoo : KillableShip
{
    [Export]
    public PackedScene DisintegrateEffect = default!;

    public AudioManager2D AudioManager2D { get; set; } = default!;
    public Events Events = default!;
    public ObjectRegistry ObjectRegistry = default!;
    public StateMachine StateMachine = default!;
    public Area2D PlanetAttackRadius = default!;
    public AnimatedSprite2D ShieldAnimation = default!;
    public Sprite2D Sprite = default!;

    private bool _shieldActive;
    private float _shield;
    private readonly int _pointValue = 15000;

    public SentientGoo()
    {
        _health = HealthMax;
    }

    public override void _Ready()
    {
        Agent.LinearAccelerationMax = AccelerationMax;
        Agent.LinearSpeedMax = LinearSpeedMax;

        Agent.AngularAccelerationMax = Mathf.DegToRad(AngularAccelerationMax);
        Agent.AngularSpeedMax = Mathf.DegToRad(AngularSpeedMax);

        Agent.LinearDragPercentage = DragFactor;
        Agent.AngularDragPercentage = AngularDragFactor;

        Sprite = GetNode<Sprite2D>("Sprite2D");
        ShieldAnimation = GetNode<AnimatedSprite2D>("ShieldAnimation");
        StateMachine = GetNode<StateMachine>("StateMachine");
        AudioManager2D = GetNode<AudioManager2D>("/root/AudioManager2D");
        ObjectRegistry = GetNode<ObjectRegistry>("/root/ObjectRegistry");

        Events = GetNode<Events>("/root/Events");

        PlanetAttackRadius = GetNode<Area2D>("PlanetAttackRadius");
        PlanetAttackRadius.Connect("body_entered", new Callable(this, "OnBodyEnteredPlanetAttackRadius"));

        ActivateShields();
    }

    public void Attack(Node2D target)
    {
        StateMachine.TransitionTo("Attack", new Dictionary<string, GodotObject> { ["target"] = target });
    }

    public void OnBodyEnteredPlanetAttackRadius(PhysicsBody2D collider)
    {
        if (collider is Planet planet)
        {
            var sprite = GetNode<Sprite2D>("Sprite2D");
            var effect = DisintegrateEffect.Instantiate<DisintegrateEffect>();
            effect.Texture = sprite.Texture;
            effect.Speed = 0.01f;
            effect.ZIndex = 100;
            effect.GlobalPosition = GlobalPosition;
            effect.GlobalRotation = GlobalRotation;
            effect.ProcessMode = ProcessModeEnum.Always;
            ObjectRegistry.RegisterEffect(effect);

            planet.TakeDamage(25);
            QueueFree();
        }
    }

    public override void TakeDamage(float damage, DamageType type)
    {
        damage = ApplyShieldDamage(damage, type);

        _health -= damage;
        if (_health <= 0)
        {
            DieByPlayer();
        }
    }

    /// <summary>
    /// Transforms and applies damage to the shield.
    /// </summary>
    /// <param name="rawDamage">The amount of incoming damage, before it's been transformed.</param>
    /// <param name="origin">An object that represents any pertinent attributes about the damage.</param>
    /// <returns>A <see cref="float"/> value representing any damage that should be applied to the shielded target.</returns>
    /// <exception cref="NotImplementedException"></exception>
    private float ApplyShieldDamage(float rawDamage, DamageType type)
    {
        if (!_shieldActive)
            return rawDamage;

        float shieldDamage;
        float targetDamage;

        switch (type)
        {
            case DamageType.Energy:
                shieldDamage = rawDamage * 0.5f;
                targetDamage = shieldDamage;
                break;
            default:
                return rawDamage * 0.25f;
        }

        _shield -= shieldDamage;
        if (_shield <= 0)
        {
            DeactivateShields();
        }

        return targetDamage;
    }

    private void ActivateShields()
    {
        Sprite.Visible = false;
        ShieldAnimation.Visible = true;
        ShieldAnimation.Play("shielded");
        _shieldActive = true;
    }

    private void DeactivateShields()
    {
        Sprite.Visible = true;
        ShieldAnimation.Visible = false;
        ShieldAnimation.Stop();
        _shieldActive = false;
    }

    private void DieByPlayer()
    {
        var sprite = GetNode<Sprite2D>("Sprite2D");

        var effect = DisintegrateEffect.Instantiate<DisintegrateEffect>();
        effect.Texture = sprite.Texture;
        effect.Speed = 0.01f;
        effect.ZIndex = 100;
        effect.GlobalPosition = GlobalPosition;
        effect.GlobalRotation = GlobalRotation;
        effect.ProcessMode = ProcessModeEnum.Always;
        ObjectRegistry.RegisterEffect(effect);
        AudioManager2D.Play(KnownAudioStream2Ds.SpaceCrabDeath, GlobalPosition);
        QueueFree();
        Events.EmitSignal("AddPoints", _pointValue);
    }
}
