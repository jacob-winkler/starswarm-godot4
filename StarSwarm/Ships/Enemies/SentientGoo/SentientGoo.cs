using System.Collections.Generic;
using Godot;
using StarSwarm.Autoload;
using StarSwarm.GSAI_Framework;
using StarSwarm.Planets;
using StarSwarm.SWStateMachine;
using StarSwarm.VFX;

namespace StarSwarm.Ships.Enemies.SentientGoo;

public partial class SentientGoo : GSAICharacterBody2D
{
    [Export]
    public PackedScene DisintegrateEffect = default!;
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

    public AudioManager2D AudioManager2D { get; set; } = default!;
    public Events Events = default!;
    public ObjectRegistry ObjectRegistry = default!;
    public StateMachine StateMachine = default!;
    public Area2D PlanetAttackRadius = default!;

    private float _health;
    private readonly int _pointValue = 15000;

    public SentientGoo()
    {
        _health = HealthMax;
    }

    public override void _Ready()
    {
        StateMachine = GetNode<StateMachine>("StateMachine");

        Agent.LinearAccelerationMax = AccelerationMax;
        Agent.LinearSpeedMax = LinearSpeedMax;

        Agent.AngularAccelerationMax = Mathf.DegToRad(AngularAccelerationMax);
        Agent.AngularSpeedMax = Mathf.DegToRad(AngularSpeedMax);

        Agent.LinearDragPercentage = DragFactor;
        Agent.AngularDragPercentage = AngularDragFactor;

        AudioManager2D = GetNode<AudioManager2D>("/root/AudioManager2D");
        ObjectRegistry = GetNode<ObjectRegistry>("/root/ObjectRegistry");
        Events = GetNode<Events>("/root/Events");
        Events.Connect("Damaged", new Callable(this, "OnDamaged"));

        PlanetAttackRadius = GetNode<Area2D>("PlanetAttackRadius");
        PlanetAttackRadius.Connect("body_entered", new Callable(this, "OnBodyEnteredPlanetAttackRadius"));
    }

    public void Attack(Node2D target)
    {
        StateMachine.TransitionTo("Attack", new Dictionary<string, GodotObject> { ["target"] = target });
    }

    public void OnBodyEnteredPlanetAttackRadius(PhysicsBody2D collider)
    {
        if (collider is Planet)
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

            Events.EmitSignal("Damaged", collider, 25, this);
            QueueFree();
        }
    }

    public void OnDamaged(Node target, float amount, Node origin)
    {
        if (target != this)
            return;

        _health -= amount;
        if (_health <= 0)
        {
            DieByPlayer();
        }
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
