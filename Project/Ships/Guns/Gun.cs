using Godot;
using StarSwarm.Project.Autoload;
using StarSwarm.Project.Ships.Guns.Projectiles;
using StarSwarm.Project.Ships.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarSwarm.Project.Ships.Guns
{
    public class Gun : Node2D
    {
        [Export]
        public PackedScene Projectile;
        [Export]
        public StatsGun Stats;

        public ObjectRegistry ObjectRegistry;
        public Timer Cooldown;

        public override void _Ready()
        {
            Stats = (StatsGun)ResourceLoader.Load("res://Project/Ships/Player/player_gun_stats.tres");
            ObjectRegistry = GetNode<ObjectRegistry>("/root/Autoload/ObjectRegistry");
            Cooldown = GetNode<Timer>("Cooldown");
            Stats.Initialize();
            Cooldown.WaitTime = Stats.GetCooldown();
        }

        public string GetConfigurationWarning() { return Projectile == null ? "Missing Projectile scene, the gun will not be able to fire" : ""; }

        public void Fire(Vector2 spawnPosition, float spawnOrientation, int projectileMask)
        {
            if (!Cooldown.IsStopped() || Projectile == null)
                return;

            var spread = Stats.GetSpread();

            var projectile = Projectile.Instance<Projectile>();
            projectile.GlobalPosition = spawnPosition;
            projectile.Rotation = spawnOrientation + Mathf.Deg2Rad(RandomSpread(spread));
            projectile.Speed *= 1.0f + RandomSpread(0.4f);
            projectile.CollisionMask = (uint)projectileMask;
            projectile.Shooter = Owner;
            projectile.Damage += Stats.GetDamage();

            ObjectRegistry.RegisterProjectiles(projectile);
            Cooldown.WaitTime = Stats.GetCooldown() * (1.0f + RandomSpread(0.2f));
            Cooldown.Start();
        }

        public void OnStatsStatChanged(string statName, float oldValue, float newValue)
        {
            switch(statName)
            {
                case ("cooldown"):
                    Cooldown.WaitTime = newValue;
                    break;
            }
        }

        public static float RandomSpread(float value)
        {
            var halfSpread = value / 2.0f;
            return (float)GD.RandRange(-halfSpread, halfSpread);
        }
    }
}
