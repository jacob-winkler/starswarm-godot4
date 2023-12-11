using Godot;
using StarSwarm.Autoload;
using StarSwarm.GSAI_Framework;
using StarSwarm.Ships.Player.States;
using StarSwarm.VFX;

namespace StarSwarm.Ships.Player;

public partial class PlayerShip : CharacterBody2D
{
	[Export]
	public PackedScene PackedDisintegrateEffect { get; set; } = default!;

	[Export]
	public StatsShip Stats { get; set; } = default!;

	[Signal]
	public delegate void DiedEventHandler();

    public AudioManager AudioManager { get; set; } = default!;
    public ObjectRegistry ObjectRegistry { get; set; } = default!;
	public Events Events { get; set; } = default!;
	public CollisionPolygon2D Shape3D { get; set; } = default!;
	public GSAISteeringAgent Agent { get; set; } = default!;
	public RemoteTransform2D CameraTransform { get; set; } = default!;
	public Move MoveState { get; set; } = default!;
	public VFX Vfx { get; set; } = default!;

	private bool _isDead;

    public override void _Ready()
	{
		AudioManager = GetNode<AudioManager>("/root/AudioManager");
		ObjectRegistry = GetNode<ObjectRegistry>("/root/ObjectRegistry");
		Events = GetNode<Events>("/root/Events");
		Shape3D = GetNode<CollisionPolygon2D>("CollisionShape3D");
		Agent = GetNode<Move>("StateMachine/Move").Agent;
		CameraTransform = GetNode<RemoteTransform2D>("CameraTransform");
		MoveState = GetNode<Move>("StateMachine/Move");
		Vfx = GetNode<VFX>("VFX");

        Events.Connect("Damaged", new Callable(this, "OnDamaged"));
		Events.Connect("UpgradeChosen", new Callable(this, "OnUpgradeChosen"));
		Stats.Connect("HealthDepleted", new Callable(this, "Die"));
	}

	public void Die()
	{
		if(_isDead)
			return;

		_isDead = true;
		EmitSignal("Died");

		var playerSprite = GetNode<Sprite2D>("Sprite2D");
		playerSprite.Visible = false;

        AudioManager.Play(KnownAudioStreams.PlayerDeath, ProcessModeEnum.Always);
        var effect = PackedDisintegrateEffect.Instantiate<DisintegrateEffect>();
		effect.Texture = playerSprite.Texture;
		effect.Speed = 0.01f;
		effect.ZIndex = 100;
		effect.GlobalPosition = GlobalPosition;
		effect.GlobalRotation = GlobalRotation;
		effect.ProcessMode = ProcessModeEnum.Always;
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
