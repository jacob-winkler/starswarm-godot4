using System.Collections.Generic;
using System.Linq;
using Godot;
using StarSwarm.Autoload;
using StarSwarm.StarSwarm.Ships;

namespace StarSwarm.Weapons.LightningRod;

public partial class LightningRodAttachment : WeaponAttachment
{
    [Export]
    public PackedScene LightningBolt { get; set; } = default!;
    [Export]
    public float MaxTargets { get; set; } = default!;
    [Export]
    public float MaxBounces { get; set; } = 2f;
    [Export]
    public float Damage { get; set; } = 100;

    public ObjectRegistry ObjectRegistry { get; set; } = default!;
    public Area2D AttackRange { get; set; } = default!;

    private readonly RandomNumberGenerator _rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        base._Ready();
        ObjectRegistry = GetNode<ObjectRegistry>("/root/ObjectRegistry");
        AttackRange = GetNode<Area2D>("AttackRange");
        CooldownTimer.Start(Cooldown);
        _rng.Randomize();
    }

    protected override void FireWeapon()
    {
        var bodiesInRange = AttackRange.GetOverlappingBodies().Cast<PhysicsBody2D>().ToList();
        for (var x = 0; x < MaxTargets; x++)
        {
            if (!bodiesInRange.Any())
                break;

            var target = (KillableShip?)bodiesInRange.Find(
                x => x.GlobalPosition.DistanceSquaredTo(GlobalPosition) == bodiesInRange.Min(x => x.GlobalPosition.DistanceSquaredTo(GlobalPosition)) && x is KillableShip);

            if (target == null)
                break;

            SpawnNewLightningBolt(target, this);
            bodiesInRange.Remove(target);
        }
    }

    private void OnBounceTriggered(LightningBolt triggeredBolt, KillableShip target)
    {
        if (triggeredBolt.BounceCount >= MaxBounces)
            return;

        triggeredBolt.ForbiddenTargets.Add(triggeredBolt.Target);

        SpawnNewLightningBolt(target, triggeredBolt.Target, triggeredBolt.TargetPosition, triggeredBolt.ForbiddenTargets,
            triggeredBolt.BounceCount + 1);
    }

    private void SpawnNewLightningBolt(KillableShip targetBody, Node2D sourceBody, Vector2 sourcePosition = default,
        List<Node2D>? forbiddenTargets = null, float bounceCount = 1)
    {
        var newLightningBolt = (LightningBolt)LightningBolt.Instantiate();
        newLightningBolt.Connect("BounceTriggered", new Callable(this, "OnBounceTriggered"));
        newLightningBolt.BounceCount = bounceCount;
        newLightningBolt.Target = targetBody;
        newLightningBolt.Source = sourceBody;
        newLightningBolt.SourcePosition = sourcePosition;
        newLightningBolt.Damage = Damage;
        newLightningBolt.ForbiddenTargets = forbiddenTargets ?? new List<Node2D>();
        ObjectRegistry.AddChild(newLightningBolt);
    }
}