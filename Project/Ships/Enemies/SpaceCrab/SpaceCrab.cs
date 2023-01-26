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
        public float HealthMax = 100f;
        [Export]
        public float LinearSpeedMax = 400f;
        [Export]
        public float AccelerationMax = 600f;
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

        public StateMachine StateMachine = null!;
        public Events Events = new Events();

        public GSAIKinematicBody2DAgent Agent { get; set; } = null!;

        public SpaceCrab()
        {
            Agent = new GSAIKinematicBody2DAgent();
            Agent.Initialize(this);
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            StateMachine = GetNode<StateMachine>("StateMachine");

            Agent.LinearAccelerationMax = AccelerationMax;
            Agent.LinearSpeedMax = LinearSpeedMax;

            Agent.AngularAccelerationMax = Mathf.Deg2Rad(AngularAccelerationMax);
            Agent.AngularSpeedMax = Mathf.Deg2Rad(AngularSpeedMax);

            Agent.LinearDragPercentage = DragFactor;
            Agent.AngularDragPercentage = AngularDragFactor;

            Events = GetNode<Events>("/root/Events");
            Events.Connect("TargetAggroed", this, "OnTargetAggroed");

            var AggroArea = GetNode<Area2D>("AggroArea");
            AggroArea.Connect("body_entered", this, "OnBodyEnteredAggroRadius");
        }

        public void OnBodyEnteredAggroRadius(PhysicsBody2D collider)
        {
            Events.EmitSignal("TargetAggroed", collider);
        }

        public void OnTargetAggroed(PhysicsBody2D target)
        {
            StateMachine.TransitionTo("Attack", new Dictionary<String, Godot.Object> { ["target"] = target });
        }
    }
}
