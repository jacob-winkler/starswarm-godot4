using Godot;
using StarSwarm.Project.Autoload;
using StarSwarm.Project.GSAI_Framework.Agents;
using StarSwarm.Project.SWStateMachine;
using System;
using System.Collections.Generic;

namespace StarSwarm.Project.Ships.Enemies.SpaceCrab;

public partial class SpaceCrab : CharacterBody2D
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

    [Export]
    public float DistanceFromTargetMin = 200f;

    [Export]
    public float DistanceFromObstaclesMin = 200f;

    [Export(PropertyHint.Layers2DPhysics)]
    public Int64 ProjectileMask = 0;

    private float _health;

    public AudioManager2D AudioManager2D { get; set; } = default!;
    public VisibleOnScreenNotifier2D VisibleOnScreenNotifier3D { get; set; } = default!;
    public StateMachine StateMachine = default!;
    public Events Events = default!;
    public ObjectRegistry ObjectRegistry = default!;
    public GSAIKinematicBody2DAgent Agent { get; set; } = default!;

    private PhysicsBody2D? _meleeTarget;
    private readonly Int32 _pointValue = 500;
    private readonly float _damagePerSecond = 500f;

    public SpaceCrab()
    {
        _health = HealthMax;
        Agent = new GSAIKinematicBody2DAgent();
        Agent.Initialize(this);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        StateMachine = GetNode<StateMachine>("StateMachine");

        VisibleOnScreenNotifier3D = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier3D");
        VisibleOnScreenNotifier3D.Connect("screen_exited", new Callable(this, "OnScreenExited"));

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

        var AggroArea = GetNode<Area2D>("AggroArea");
        AggroArea.Connect("body_entered", new Callable(this, "OnBodyEnteredAggroRadius"));

        var MeleeRange = GetNode<Area2D>("MeleeRange");
        MeleeRange.Connect("body_entered", new Callable(this, "OnBodyEnteredMeleeRange"));
        MeleeRange.Connect("body_exited", new Callable(this, "OnBodyExitedMeleeRange"));
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_meleeTarget != null)
        {
            Events.EmitSignal("Damaged", _meleeTarget, _damagePerSecond * delta, this);
        }
    }

    public void OnBodyEnteredAggroRadius(PhysicsBody2D collider) =>
        StateMachine.TransitionTo("Attack", new Dictionary<String, GodotObject> { ["target"] = collider });

    public void OnBodyEnteredMeleeRange(PhysicsBody2D playerBody) =>
        _meleeTarget = playerBody;

    public void OnBodyExitedMeleeRange(PhysicsBody2D playerBody) =>
        _meleeTarget = null;

    public void OnScreenExited() => Events.EmitSignal("EnemyAdrift", this);

    public void OnDamaged(Node target, float amount, Node origin)
    {
        if (target != this)
            return;

        _health -= amount;
        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        var effect = (Node2D)DisintegrateEffect.Instantiate();
        effect.GlobalPosition = GlobalPosition;
        effect.GlobalRotation = GlobalRotation;
        ObjectRegistry.RegisterEffect(effect);
        AudioManager2D.Play(KnownAudioStream2Ds.SpaceCrabDeath, GlobalPosition);
        QueueFree();
        Events.EmitSignal("SpaceCrabDied");
        Events.EmitSignal("AddPoints", _pointValue);
    }
}