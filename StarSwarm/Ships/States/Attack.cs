using System;
using System.Collections.Generic;
using Godot;
using StarSwarm.GSAI_Framework;
using StarSwarm.GSAI_Framework.Behaviors;

namespace StarSwarm.Ships.States;

public partial class Attack<TShip> : ShipState<TShip>
	where TShip : GSAICharacterBody2D
{
	[Export]
	public float MinDistanceFromTarget = 0f;

    private GSAISteeringAgent _target = default!;
    private GSAIPursue _pursue = default!;
    private GSAIFace _face = default!;
    private GSAIBlend _blend = default!;
    private GSAITargetAcceleration Acceleration { get; set; } = new GSAITargetAcceleration();

	public override void _Ready()
	{
		base._Ready();
		_pursue = new GSAIPursue(Ship.Agent, _target);
		_face = new GSAIFace(Ship.Agent, _target);
		_blend = new GSAIBlend(Ship.Agent);

		_blend.Add(_pursue, 1);
		_blend.Add(_face, 1);
	}

	public override void Enter(Dictionary<string, GodotObject>? msg = null)
	{
		if(msg == null) return;

        var agent = GetAgent(msg["target"]);
        if (agent == null)
            throw new NullReferenceException($"Target agent is null. Target must have an \"Agent\" property of type {typeof(GSAISteeringAgent)}");

        _target = agent;
		_pursue.Target = _target;
		_face.Target = _target;
	}

	public override void PhysicsProcess(double delta)
	{
		_blend.CalculateSteering(Acceleration);
		Ship.Agent.ApplySteering(Acceleration, delta);
		var facingDirection = GSAIUtils.AngleToVector2(Ship.Agent.Orientation);
		var toPlayer = GSAIUtils.ToVector2(Ship.Agent.Position - _target.Position).Normalized();
		var playerDotFacing = facingDirection.Dot(toPlayer);
	}

    private GSAISteeringAgent? GetAgent(GodotObject gobject)
    {
        GSAISteeringAgent? agent = null;

        var type = gobject.GetType();
        var agentProperty = type.GetProperty("Agent");
        if (agentProperty != null)
        {
            if(agentProperty.PropertyType.IsAssignableTo(typeof(GSAISteeringAgent)))
            {
                agent = (GSAISteeringAgent?)agentProperty.GetValue(gobject);
            }
        }

        return agent;
    }
}
