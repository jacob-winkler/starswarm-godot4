using Godot;
using StarSwarm.Project.Autoload;
using StarSwarm.Project.Ships.Enemies.SpaceCrab.States;
using System;
using System.Collections.Generic;
using StarSwarm.Project.SWStateMachine;
using StarSwarm.Project.GSAI_Framework.Agents;
using StarSwarm.Project.Utils;

namespace StarSwarm.Project.Ships.Enemies.SpaceCrab
{
    public class SpaceCrab : KinematicBody2D
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
        [Export(PropertyHint.Layers2dPhysics)]
        public Int64 ProjectileMask = 0;

        private float _health;

        public VisibilityNotifier2D VisibilityNotifier { get; set; } = default!;
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

            VisibilityNotifier = GetNode<VisibilityNotifier2D>("VisibilityNotifier");
            VisibilityNotifier.Connect("screen_exited", this, "OnScreenExited");

            Agent.LinearAccelerationMax = AccelerationMax;
            Agent.LinearSpeedMax = LinearSpeedMax;

            Agent.AngularAccelerationMax = Mathf.Deg2Rad(AngularAccelerationMax);
            Agent.AngularSpeedMax = Mathf.Deg2Rad(AngularSpeedMax);

            Agent.LinearDragPercentage = DragFactor;
            Agent.AngularDragPercentage = AngularDragFactor;

            ObjectRegistry = GetNode<ObjectRegistry>("/root/ObjectRegistry");
            Events = GetNode<Events>("/root/Events");
            Events.Connect("Damaged", this, "OnDamaged");

            var AggroArea = GetNode<Area2D>("AggroArea");
            AggroArea.Connect("body_entered", this, "OnBodyEnteredAggroRadius");

            var MeleeRange = GetNode<Area2D>("MeleeRange");
            MeleeRange.Connect("body_entered", this, "OnBodyEnteredMeleeRange");
            MeleeRange.Connect("body_exited", this, "OnBodyExitedMeleeRange");
        }

        public override void _PhysicsProcess(float delta)
        {
            if (_meleeTarget != null)
            {
                Events.EmitSignal("Damaged", _meleeTarget, _damagePerSecond * delta, this);
            }
        }

        public void OnBodyEnteredAggroRadius(PhysicsBody2D collider)
        {
            StateMachine.TransitionTo("Attack", new Dictionary<String, Godot.Object> { ["target"] = collider });
        }

        public void OnBodyEnteredMeleeRange(PhysicsBody2D playerBody)
        {
            _meleeTarget = playerBody;
        }

        public void OnBodyExitedMeleeRange(PhysicsBody2D playerBody)
        {
            _meleeTarget = null;
        }

        public void OnScreenExited()
        {
            Events.EmitSignal("EnemyAdrift", this);
        }

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
            var effect = (Node2D)DisintegrateEffect.Instance();
            effect.GlobalPosition = GlobalPosition;
            effect.GlobalRotation = GlobalRotation;
            ObjectRegistry.RegisterEffect(effect);
            QueueFree();
            Events.EmitSignal("SpaceCrabDied");
            Events.EmitSignal("AddPoints", _pointValue);
        }
    }
}
