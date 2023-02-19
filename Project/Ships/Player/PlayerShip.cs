using Godot;
using StarSwarm.Project.Autoload;
using StarSwarm.Project.GSAI_Framework;
using StarSwarm.Project.GSAI_Framework.Agents;
using StarSwarm.Project.Ships.Player;
using StarSwarm.Project.Ships.Player.States;
using System;

public class PlayerShip : KinematicBody2D
{
	[Export]
	public StatsShip Stats = (StatsShip)ResourceLoader.Load("res://Project/Ships/Player/player_stats.tres");
	
	[Export]
	public PackedScene ExplosionEffect = default!;

	[Signal]
	public delegate void Died();

	public ObjectRegistry ObjectRegistry = default!;
	public Events Events = default!;
	public CollisionPolygon2D Shape = default!;
	public GSAISteeringAgent Agent = default!;
	public RemoteTransform2D CameraTransform = default!;
	public Move MoveState = default!;
	public VFX Vfx = default!;

	public override void _Ready()
	{
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
