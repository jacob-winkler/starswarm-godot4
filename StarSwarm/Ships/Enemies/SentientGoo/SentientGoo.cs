using System.Collections.Generic;
using Godot;
using StarSwarm.GSAI_Framework;
using StarSwarm.SWStateMachine;

namespace StarSwarm.Ships.Enemies.SentientGoo;

public partial class SentientGoo : GSAICharacterBody2D
{
    public StateMachine StateMachine = default!;

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

    private float _health;

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
    }

    public void Attack(Node2D target)
    {
        StateMachine.TransitionTo("Attack", new Dictionary<string, GodotObject> { ["target"] = target });
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
}
