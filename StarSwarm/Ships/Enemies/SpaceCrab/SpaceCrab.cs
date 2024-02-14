using System.Collections.Generic;
using Godot;
using StarSwarm.Autoload;
using StarSwarm.SWStateMachine;
using StarSwarm.Weapons;

namespace StarSwarm.Ships.Enemies.SpaceCrab;

public partial class SpaceCrab : KillableShip
	{
		[Export]
		public PackedScene DisintegrateEffect = default!;

		public AudioManager2D AudioManager2D { get; set; } = default!;
		public VisibleOnScreenNotifier2D VisibleOnScreenNotifier3D { get; set; } = default!;
		public StateMachine StateMachine = default!;
		public Events Events = default!;
		public ObjectRegistry ObjectRegistry = default!;

		private KillableShip? _meleeTarget;
		private readonly int _pointValue = 500;
		private readonly float _damagePerSecond = 500f;

		public SpaceCrab()
		{
			_health = HealthMax;
		}

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			StateMachine = GetNode<StateMachine>("StateMachine");

			VisibleOnScreenNotifier3D = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier3D");
			VisibleOnScreenNotifier3D.Connect("screen_exited", new Callable(this, "OnScreenExited"));

			Agent.LinearAccelerationMax = AccelerationMax;
			Agent.LinearSpeedMax = LinearSpeedMax;

			Agent.AngularAccelerationMax = Mathf.DegToRad(AngularAccelerationMax);
			Agent.AngularSpeedMax = Mathf.DegToRad(AngularSpeedMax);

			Agent.LinearDragPercentage = DragFactor;
			Agent.AngularDragPercentage = AngularDragFactor;

			AudioManager2D = GetNode<AudioManager2D>("/root/AudioManager2D");
			ObjectRegistry = GetNode<ObjectRegistry>("/root/ObjectRegistry");
			Events = GetNode<Events>("/root/Events");

			var AggroArea = GetNode<Area2D>("AggroArea");
			AggroArea.Connect("body_entered", new Callable(this, "OnBodyEnteredAggroRadius"));

			var MeleeRange = GetNode<Area2D>("MeleeRange");
			MeleeRange.Connect("body_entered", new Callable(this, "OnBodyEnteredMeleeRange"));
			MeleeRange.Connect("body_exited", new Callable(this, "OnBodyExitedMeleeRange"));
		}

		public override void _PhysicsProcess(double delta)
		{
			_meleeTarget?.TakeDamage(_damagePerSecond * ((float)delta), DamageType.Physical);
		}

		public void OnBodyEnteredAggroRadius(PhysicsBody2D collider)
		{
			StateMachine.TransitionTo("Attack", new Dictionary<string, GodotObject> { ["target"] = collider });
		}

		public void OnBodyEnteredMeleeRange(PhysicsBody2D playerBody)
		{
            if(playerBody is KillableShip player)
            {
                _meleeTarget = player;
            }
		}

		public void OnBodyExitedMeleeRange(PhysicsBody2D playerBody)
		{
			_meleeTarget = null;
		}

		public void OnScreenExited()
		{
			Events.EmitSignal("EnemyAdrift", this);
		}

        public override void TakeDamage(float damage, DamageType type)
        {
            _health -= damage;
            if (_health <= 0)
            {
                Die();
            }
        }

        private void Die()
		{
			var effect = (Node2D)DisintegrateEffect.Instantiate();
			effect.GlobalPosition = GlobalPosition;
			effect.GlobalRotation = GlobalRotation;
			ObjectRegistry.RegisterEffect(effect);
			AudioManager2D.Play(KnownAudioStream2Ds.SpaceCrabDeath, GlobalPosition);
			QueueFree();
			Events.EmitSignal("SpaceCrabDied");
			Events.EmitSignal("AddPoints", _pointValue);
		}
	}
