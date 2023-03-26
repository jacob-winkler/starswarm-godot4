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

                var lightningBolt = (LightningBolt)LightningBolt.Instance();
                lightningBolt.Connect("BounceTriggered", this, "OnBounceTriggered");
                lightningBolt.Position = Position - GlobalPosition;
                lightningBolt.Target = target;
                lightningBolt.Source = this;
                ObjectRegistry.AddChild(lightningBolt);
                bodiesInRange.Remove(target);
            }
        }

        private void OnBounceTriggered(LightningBolt triggeredBolt, PhysicsBody2D target)
        {
            if (triggeredBolt.BounceCount >= MaxBounces)
                return;

            var newLightningBolt = (LightningBolt)LightningBolt.Instance();
            newLightningBolt.BounceCount = triggeredBolt.BounceCount++;
            newLightningBolt.Position = Position - GlobalPosition;
            newLightningBolt.Target = target;
            newLightningBolt.Source = triggeredBolt.Target;
            newLightningBolt.SourcePosition = triggeredBolt.TargetPosition;
            ObjectRegistry.AddChild(newLightningBolt);
        }
    }
}