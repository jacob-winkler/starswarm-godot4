using Godot;
using StarSwarm.Project.Autoload;
using StarSwarm.Project.Ships.Enemies.SpaceCrab.States;
using System;
using System.Collections.Generic;
using StarSwarm.Project.SWStateMachine;
using StarSwarm.Project.GSAI_Framework.Agents;

namespace StarSwarm.Project.Ships.Enemies.SpaceCrab
{
    public class SpaceCrab : KinematicBody2D
    {
        public StateMachine StateMachine = new StateMachine();
        public Events Events = new Events();

        public GSAIKinematicBody2DAgent Agent { get; set; } = new GSAIKinematicBody2DAgent();

        public SpaceCrab()
        {
            Agent = new GSAIKinematicBody2DAgent(this);
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
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
