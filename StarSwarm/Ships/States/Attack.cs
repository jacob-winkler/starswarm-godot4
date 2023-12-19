using System.Collections.Generic;
using Godot;
using StarSwarm.GSAI_Framework;
using StarSwarm.GSAI_Framework.Behaviors;
using StarSwarm.Ships.Player;

namespace StarSwarm.Ships.States;

public partial class Attack<TShip> : ShipState<TShip>
		where TShip : GSAICharacterBody2D
	{
		[Export]
		public float MinDistanceFromTarget = 0f;

    private GSAISteeringAgent Target { get; set; } = default!;
    private GSAIPursue Pursue { get; set; } = default!;
    private GSAIFace Face { get; set; } = default!;
    private GSAIBlend Blend { get; set; } = default!;
    private GSAITargetAcceleration Acceleration { get; set; } = new GSAITargetAcceleration();

		public override void _Ready()
		{
			base._Ready();
			Pursue = new GSAIPursue(Ship.Agent, Target);
			Face = new GSAIFace(Ship.Agent, Target);
			Blend = new GSAIBlend(Ship.Agent);

			Blend.Add(Pursue, 1);
			Blend.Add(Face, 1);
		}

		public override void Enter(Dictionary<string, GodotObject>? msg = null)
		{
			if(msg == null) return;

			Target = ((PlayerShip)msg["target"]).Agent;
			Pursue.Target = Target;
			Face.Target = Target;
		}

		public override void PhysicsProcess(double delta)
		{
			Blend.CalculateSteering(Acceleration);
			Ship.Agent.ApplySteering(Acceleration, delta);
			var facingDirection = GSAIUtils.AngleToVector2(Ship.Agent.Orientation);
			var toPlayer = GSAIUtils.ToVector2(Ship.Agent.Position - Target.Position).Normalized();
			var playerDotFacing = facingDirection.Dot(toPlayer);
		}
	}