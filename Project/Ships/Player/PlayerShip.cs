using System;
using Godot;
using StarSwarm.Project.Autoload;
using StarSwarm.Project.GSAI_Framework;
using StarSwarm.Project.Ships.Player;
using StarSwarm.Project.Ships.Player.States;

public class PlayerShip : KinematicBody2D
{
	[Export]
	public PackedScene PackedDisintegrateEffect { get; set; } = default!;

	[Export]
	public StatsShip Stats { get; set; } = default!;

	[Signal]
	public delegate void Died();

    public AudioManager AudioManager { get; set; } = default!;
    public ObjectRegistry ObjectRegistry { get; set; } = default!;
	public Events Events { get; set; } = default!;
	public CollisionPolygon2D Shape { get; set; } = default!;
	public GSAISteeringAgent Agent { get; set; } = default!;
	public RemoteTransform2D CameraTransform { get; set; } = default!;
	public Move MoveState { get; set; } = default!;
	public VFX Vfx { get; set; } = default!;

	private Boolean _isDead;

    public override void _Ready()
	{
		AudioManager = GetNode<AudioManager>("/root/AudioManager");
		ObjectRegistry = GetNode<ObjectRegistry>("/root/ObjectRegistry");
		Events = GetNode<Events>("/root/Events");
		Shape = GetNode<CollisionPolygon2D>("CollisionShape");
		Agent = GetNode<Move>("StateMachine/Move").Agent;
		CameraTransform = GetNode<RemoteTransform2D>("CameraTransform");
		MoveState = GetNode<Move>("StateMachine/Move");
		Vfx = GetNode<VFX>("VFX");

        Events.Connect("Damaged", this, "OnDamaged");
		Events.Connect("UpgradeChosen", this, "OnUpgradeChosen");
		Stats.Connect("HealthDepleted", this, "Die");
		Stats.Initialize();
	}

	public void Die()
	{
		if(_isDead)
			return;

		_isDead = true;
		EmitSignal("Died");

		var playerSprite = GetNode<Sprite>("Sprite");
		playerSprite.Visible = false;

        AudioManager.Play(KnownAudioStreams.PlayerDeath, PauseModeEnum.Process);
        var effect = PackedDisintegrateEffect.Instance<DisintegrateEffect>();
		effect.Texture = playerSprite.Texture;
		effect.Speed = 0.01f;
		effect.ZIndex = 100;
		effect.GlobalPosition = GlobalPosition;
		effect.GlobalRotation = GlobalRotation;
		effect.PauseMode = PauseModeEnum.Process;
		ObjectRegistry.RegisterEffect(effect);
	}

	public void GrabCamera(Camera2D camera)
	{
		CameraTransform.RemotePath = camera.GetPath();
	}

	public void OnDamaged(Node target, float amount, Node origin)
	{
		if (target != this)
			return;

		Stats.Health -= amount;
	}


	public void OnUpgradeChosen(int choice)
	{
		switch(choice)
		{
			case (int)UpgradeChoices.HEALTH:
				Stats.AddModifier("maxHealth", 25.0F);
				break;
			case (int)UpgradeChoices.SPEED:
				Stats.AddModifier("linearSpeedMax", 125.0F);
				break;
		}
	}
}
