﻿using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarSwarm.Project.GSAI_Framework.Agents
{
    public class GSAIKinematicBody2DAgent : GSAISpecializedAgent
    {
        protected KinematicBody2D body = null;
        protected KnownMovementType MovementType { get; set; }

        private Vector2 _lastPosition;
        private WeakRef _bodyRef;
        public KinematicBody2D Body
        {
            set
            {
                body = value;
                _bodyRef = WeakRef(body);

                _lastPosition = value.GlobalPosition;
                _lastOrientation = value.Rotation;

                position = GSAIUtils.ToVector3(_lastPosition);
                orientation = _lastOrientation;
            }

            get { return body; }
        }

        public GSAIKinematicBody2DAgent(KinematicBody2D body, KnownMovementType movementType = KnownMovementType.Slide)
        {
            Body = body;
            MovementType = movementType;

            body.GetTree().Connect("physics_frame", this, "OnSceneTreePhysicsFrame");
        }

        public override void ApplySteering(GSAITargetAcceleration acceleration, float delta)
        {
            _appliedSteering = true;
            switch(MovementType)
            {
                case KnownMovementType.Collide:
                    ApplyCollideSteering(acceleration.Linear, delta);
                    break;
                case KnownMovementType.Slide:
                    ApplySlidingSteering(acceleration.Linear, delta);
                    break;
                default:
                    ApplyPositionSteering(acceleration.Linear, delta);
                    break;
            }

            ApplyOrientationSteering(acceleration.Angular, delta);
        }

        public void ApplySlidingSteering(Vector3 accel, float delta)
        {
            KinematicBody2D body = (KinematicBody2D)_bodyRef.GetRef();
            if (body == null)
                return;

            var velocity = GSAIUtils.ToVector2(LinearVelocity + accel * delta).Clamped(LinearSpeedMax);
            if(ApplyLinearDrag)
                velocity = velocity.LinearInterpolate(Vector2.Zero, LinearDragPercentage);
            velocity = body.MoveAndSlide(velocity);
            if(CalculateVelocities)
                LinearVelocity = GSAIUtils.ToVector3(velocity);
        }

        public void ApplyCollideSteering(Vector3 accel, float delta)
        {
            KinematicBody2D body = (KinematicBody2D)_bodyRef.GetRef();
            if (body == null)
                return;

            var velocity = GSAIUtils.ClampedV3(LinearVelocity + accel * delta, LinearSpeedMax);
            if (ApplyLinearDrag)
                velocity = velocity.LinearInterpolate(Vector3.Zero, LinearDragPercentage);

            body.MoveAndCollide(GSAIUtils.ToVector2(velocity) * delta);
            if (CalculateVelocities)
                LinearVelocity = velocity;
        }

        public void ApplyPositionSteering(Vector3 accel, float delta)
        {
            KinematicBody2D body = (KinematicBody2D)_bodyRef.GetRef();
            if (body == null)
                return;

            var velocity = GSAIUtils.ClampedV3(LinearVelocity + accel * delta, LinearSpeedMax);
            if (ApplyLinearDrag)
                velocity = velocity.LinearInterpolate(Vector3.Zero, LinearDragPercentage);

            body.GlobalPosition += GSAIUtils.ToVector2(velocity) * delta;
            if (CalculateVelocities)
                LinearVelocity = velocity;
        }

        public void ApplyOrientationSteering(float angular_acceleration, float delta)
        {
            KinematicBody2D body = (KinematicBody2D)_bodyRef.GetRef();
            if (body == null)
                return;

            var velocity = Mathf.Clamp(
                AngularVelocity + angular_acceleration * delta,
                -AngularAccelerationMax,
                AngularAccelerationMax
            );
            if (ApplyLinearDrag)
                velocity = Mathf.Lerp(velocity, 0, AngularDragPercentage);

            body.Rotation += velocity * delta;
            if (CalculateVelocities)
                AngularVelocity = velocity;
        }
    }
}