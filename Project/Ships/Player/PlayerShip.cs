using Godot;
using StarSwarm.Project.Autoload;
using StarSwarm.Project.GSAI_Framework;
using StarSwarm.Project.GSAI_Framework.Agents;
using StarSwarm.Project.Ships.Guns;
using StarSwarm.Project.Ships.Player;
using StarSwarm.Project.Ships.Player.States;
using System;

public class PlayerShip : KinematicBody2D
{
	[Export]
	public StatsShip Stats = (StatsShip)ResourceLoader.Load("res://Project/Ships/Player/player_stats.tres");
	
	[Export]
	public PackedScene ExplosionEffect = new PackedScene();

	[Signal]
	public delegate void Died();

	public ObjectRegistry ObjectRegistry = new ObjectRegistry();
	public Events Events = new Events();
	public CollisionPolygon2D Shape = new CollisionPolygon2D();
	public GSAISteeringAgent Agent = new GSAISteeringAgent();
	public RemoteTransform2D CameraTransform = new RemoteTransform2D();
	public Move MoveState = new Move();
	public Gun Gun = new Gun();
	public VFX Vfx = new VFX();

	public override void _Ready()
	{
		ObjectRegistry = GetNode<ObjectRegistry>("/root/ObjectRegistry");
		Events = GetNode<Events>("/root/Events");
		Shape = GetNode<CollisionPolygon2D>("CollisionShape");
		Agent = GetNode<Move>("StateMachine/Move").Agent;
		CameraTransform = GetNode<RemoteTransform2D>("CameraTransform");
		MoveState = GetNode<Move>("StateMachine/Move");
		Gun = GetNode<Gun>("Gun");
		Vfx = GetNode<VFX>("VFX");
		Events.Connect("Damaged", this, "OnDamaged");
		Events.Connect("UpgradeChosen", this, "OnUpgradeChosen");
		Stats.Connect("HealthDepleted", this, "Die");
		Stats.Initialize();
	}

	public void Die()
	{
		var effect = ExplosionEffect.Instance<Node2D>();
		effect.GlobalPosition = GlobalPosition;
		ObjectRegistry.RegisterEffect(effect);

		EmitSignal("Died");
		Events.EmitSignal("PlayerDied");

		QueueFree();
	}


	public void GrabCamera(Camera2D camera)
	{
		CameraTransform.RemotePath = camera.GetPath();
	}


	public void OnDamaged(Node target, int amount, Node origin)
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
			case (int)UpgradeChoices.WEAPON:
				Gun.Stats.AddModifier("damage", 3.0F);
				// That's a limitation of the stats system, modifiers only add or remove values, and they
				// don't have limits
				if (Gun.Stats.GetStat("cooldown") > 2.0f)
					Gun.Stats.AddModifier("cooldown", -0.05f);
				break;
		}
	}
}
