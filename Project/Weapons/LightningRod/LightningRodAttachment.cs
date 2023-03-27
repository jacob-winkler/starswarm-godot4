using Godot;
using StarSwarm.Project.Autoload;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarSwarm.Project.Weapons.LightningRod
{
    public class LightningRodAttachment : WeaponAttachment
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
            _cooldownTimer.Start(Cooldown);
            _rng.Randomize();
        }

        protected override void FireWeapon()
        {
            var bodiesInRange = AttackRange.GetOverlappingBodies().Cast<PhysicsBody2D>().ToList();
            for (var x = 0; x < MaxTargets; x++)
            {
                if(!bodiesInRange.Any())
                    break;

                var target = bodiesInRange.Find(
                    x => x.GlobalPosition.DistanceSquaredTo(GlobalPosition) == bodiesInRange.Min(x => x.GlobalPosition.DistanceSquaredTo(GlobalPosition)));

                SpawnNewLightningBolt(target, this);
                bodiesInRange.Remove(target);
            }
        }

        private void OnBounceTriggered(LightningBolt triggeredBolt, PhysicsBody2D target)
        {
            if (triggeredBolt.BounceCount >= MaxBounces)
                return;

            SpawnNewLightningBolt(target, triggeredBolt.Target, triggeredBolt.TargetPosition, triggeredBolt.BounceCount + 1);
        }

        private void SpawnNewLightningBolt(Node2D targetBody, Node2D sourceBody, Vector2 sourcePosition = default, float bounceCount = 1)
        {
            var newLightningBolt = (LightningBolt)LightningBolt.Instance();
            newLightningBolt.Connect("BounceTriggered", this, "OnBounceTriggered");
            newLightningBolt.BounceCount = bounceCount;
            newLightningBolt.Target = targetBody;
            newLightningBolt.Source = sourceBody;
            newLightningBolt.SourcePosition = sourcePosition;
            newLightningBolt.Damage = Damage;
            ObjectRegistry.AddChild(newLightningBolt);
        }
    }
}