using Godot;
using StarSwarm.Project.GSAI_Framework.Agents;
using StarSwarm.Project.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarSwarm.Project.Ships.Player.States
{
    public class Move : PlayerState
    {
        [Export]
        public float DragLinearCoefficient = 0.05F;
        [Export]
        public float ReverseMultiplier = 0.25F;
        [Export]
        public float DragAngularCoeff = 0.1F;

        public float AccelerationMax = 0.0F;
        public float LinearSpeedMax = 1.2F;
        public float AngularSpeedMax = 0.0F;
        public float AngularAccelerationMax = 0.0F;

        public Vector2 LinearVelocity = Vector2.Zero;
        public float AngularVelocity = 0.0F;
        public bool IsReversing = false;
        public bool CanFire = true;

        public GSAIKinematicBody2DAgent Agent;

        public override async void _Ready()
        {
            base._Ready();
            
            Agent = new GSAIKinematicBody2DAgent((KinematicBody2D)Owner);

            await ToSignal(Owner, "ready");

            AccelerationMax = Ship.Stats.GetAccelerationMax();
            LinearSpeedMax = Ship.Stats.GetLinearSpeedMax();
            AngularSpeedMax = Ship.Stats.GetAngularSpeedMax();
            AngularAccelerationMax = Ship.Stats.GetAngularAccelerationMax();

            Agent.LinearAccelerationMax = AccelerationMax * ReverseMultiplier;
            Agent.LinearSpeedMax = LinearSpeedMax;
            Agent.AngularAccelerationMax = Mathf.Deg2Rad(AngularAccelerationMax);
            Agent.AngularSpeedMax = Mathf.Deg2Rad(AngularSpeedMax);
            Agent.BoundingRadius = MathUtils.GetTriangleCircumcircleRadius(Ship.Shape.Polygon);
        }

        public override void PhysicsProcess(float delta)
        {
            LinearVelocity = LinearVelocity.Clamped(LinearSpeedMax);
            LinearVelocity = LinearVelocity.LinearInterpolate(Vector2.Zero, DragLinearCoefficient);

            AngularVelocity = Mathf.Clamp(AngularVelocity, -Agent.AngularSpeedMax, Agent.AngularSpeedMax);
            AngularVelocity = Mathf.Lerp(AngularVelocity, 0, DragAngularCoeff);

            LinearVelocity = Ship.MoveAndSlide(LinearVelocity);
            Ship.Rotation += AngularVelocity * delta;
            Ship.Vfx.MakeTrail(LinearVelocity.Length());
        }
    }
}
