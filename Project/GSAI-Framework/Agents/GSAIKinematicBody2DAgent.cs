using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarSwarm.Project.GSAI_Framework.Agents;

	public partial class GSAIKinematicBody2DAgent : GSAISpecializedAgent
	{
		protected CharacterBody2D body = default!;
		protected KnownMovementType MovementType { get; set; }

		private Vector2 _lastPosition { get; set; } = default!;
		private WeakRef _bodyRef { get; set; } = default!;
		public CharacterBody2D Body
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

		public async void Initialize(CharacterBody2D body,
			KnownMovementType movementType = KnownMovementType.Slide)
		{
			if(!body.IsInsideTree())
				await ToSignal(body, "ready");

			_bodyRef = WeakRef(body);
			MovementType = movementType;

			body.GetTree().Connect("physics_frame", new Callable(this, "OnSceneTreePhysicsFrame"));
		}

		public override void ApplySteering(GSAITargetAcceleration acceleration, double delta)
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

		public void ApplySlidingSteering(Vector3 accel, double delta)
		{
			CharacterBody2D body = (CharacterBody2D)_bodyRef.GetRef();
			if (body == null)
				return;

			var velocity = GSAIUtils.ToVector2(LinearVelocity + accel * (float)delta).LimitLength(LinearSpeedMax);
			if(ApplyLinearDrag)
				velocity = velocity.Lerp(Vector2.Zero, LinearDragPercentage);

			body.Velocity = velocity;
			body.MoveAndSlide();

			if (CalculateVelocities)
				LinearVelocity = GSAIUtils.ToVector3(body.Velocity);
		}

		public void ApplyCollideSteering(Vector3 accel, double delta)
		{
			CharacterBody2D body = (CharacterBody2D)_bodyRef.GetRef();
			if (body == null)
				return;

			var velocity = GSAIUtils.ClampedV3(LinearVelocity + accel * (float)delta, LinearSpeedMax);
			if (ApplyLinearDrag)
				velocity = velocity.Lerp(Vector3.Zero, LinearDragPercentage);

			body.MoveAndCollide(GSAIUtils.ToVector2(velocity) * (float)delta);
			if (CalculateVelocities)
				LinearVelocity = velocity;
		}

		public void ApplyPositionSteering(Vector3 accel, double delta)
		{
			CharacterBody2D body = (CharacterBody2D)_bodyRef.GetRef();
			if (body == null)
				return;

			var velocity = GSAIUtils.ClampedV3(LinearVelocity + accel * (float)delta, LinearSpeedMax);
			if (ApplyLinearDrag)
				velocity = velocity.Lerp(Vector3.Zero, LinearDragPercentage);

			body.GlobalPosition += GSAIUtils.ToVector2(velocity) * (float)delta;
			if (CalculateVelocities)
				LinearVelocity = velocity;
		}

		public void ApplyOrientationSteering(float angular_acceleration, double delta)
		{
			CharacterBody2D body = (CharacterBody2D)_bodyRef.GetRef();
			if (body == null)
				return;

			var velocity = (float)Mathf.Clamp(
				AngularVelocity + angular_acceleration * delta,
				-AngularAccelerationMax,
				AngularAccelerationMax
			);
			if (ApplyLinearDrag)
				velocity = Mathf.Lerp(velocity, 0, AngularDragPercentage);

			body.Rotation += velocity * (float)delta;
			if (CalculateVelocities)
				AngularVelocity = velocity;
		}

		public void OnSceneTreePhysicsFrame()
		{
			var body = _bodyRef.GetRef();
			if(body.VariantType == Variant.Type.Nil)
				return;

			var currentPosition = ((CharacterBody2D)body).GlobalPosition;
			var currentOrientation = ((CharacterBody2D)body).Rotation;

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
						LinearVelocity = LinearVelocity.Lerp(
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
