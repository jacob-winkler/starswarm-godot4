using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarSwarm.Project.GSAI_Framework.Agents
{
	public class GSAIKinematicBody2DAgent : GSAISpecializedAgent
	{
		protected KinematicBody2D body = default!;
		protected KnownMovementType MovementType { get; set; }

		private Vector2 _lastPosition { get; set; } = default!;
		private WeakRef _bodyRef { get; set; } = default!;
		public KinematicBody2D Body
		{
			set
			{
				body = value;
				_bodyRef = WeakRef(body);

				_lastPosition = value.GlobalPosition;
				_lastOrientation = value.Rotation;

				Position = GSAIUtils.ToVector3(_lastPosition);
				Orientation = _lastOrientation;
			}

			get { return body; }
		}

		public GSAIKinematicBody2DAgent()
		{ }

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

		public void OnSceneTreePhysicsFrame()
		{
			var body = _bodyRef.GetRef();
			if(body == null)
				return;

			var currentPosition = ((KinematicBody2D)body).GlobalPosition;
			var currentOrientation = ((KinematicBody2D)body).Rotation;

			Position = GSAIUtils.ToVector3(currentPosition);
			Orientation = currentOrientation;

			if (CalculateVelocities)
			{
				if (_appliedSteering)
					_appliedSteering = false;
				else
				{
					LinearVelocity = GSAIUtils.ClampedV3(
						GSAIUtils.ToVector3(_lastPosition - currentPosition), LinearSpeedMax
					);
					if (ApplyLinearDrag)
					{
						LinearVelocity = LinearVelocity.LinearInterpolate(
							Vector3.Zero, LinearDragPercentage);
					}

					AngularVelocity = Mathf.Clamp(
						_lastOrientation - currentOrientation, -AngularSpeedMax, AngularSpeedMax
					);

					if (ApplyAngularDrag)
						AngularVelocity = Mathf.Lerp(AngularVelocity, 0, AngularDragPercentage);

					_lastPosition = currentPosition;
					_lastOrientation = currentOrientation;                  
				}
			}
		}
	}
}
