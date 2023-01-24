using Godot;
using StarSwarm.Project.GSAI_Framework;
using StarSwarm.Project.GSAI_Framework.Behaviors;
using System;
using System.Collections.Generic;

namespace StarSwarm.Project.Ships.Enemies.SpaceCrab.States
{
    public class Attack : SpaceCrabState
    {
        [Export]
        public float MinDistanceFromTarget = 0f;

        GSAISteeringAgent Target { get; set; } = default!;
        GSAIPursue Pursue { get; set; } = default!;
        GSAIFace Face { get; set; } = default!;
        GSAIBlend Blend { get; set; } = default!;
        GSAITargetAcceleration Acceleration { get; set; } = new GSAITargetAcceleration();

        public async override void _Ready()
        {
            await ToSignal(Owner, "ready");
            Pursue = new GSAIPursue(Ship.Agent, Target);
            Face = new GSAIFace(Ship.Agent, Target);
            Blend = new GSAIBlend(Ship.Agent);

            Blend.Add(Pursue, 1);
            Blend.Add(Face, 1);
        }

        public override void Enter(Dictionary<String, Godot.Object>? msg = null)
        {
            if(msg == null) return;

            Target = (GSAISteeringAgent)msg["target"];
            Pursue.Target = Target;
            Face.Target = Target;
        }

        public override void PhysicsProcess(float delta)
        {
            Blend.CalculateSteering(Acceleration);
            Ship.Agent.ApplySteering(Acceleration, delta);
            var facingDirection = GSAIUtils.AngleToVector2(Ship.Agent.Orientation);
            var toPlayer = GSAIUtils.ToVector2(Ship.Agent.Position - Target.Position).Normalized();
            var playerDotFacing = facingDirection.Dot(toPlayer);
        }
    }
}