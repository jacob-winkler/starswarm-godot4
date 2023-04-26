using Godot;
using StarSwarm.Project.Planets;
using StarSwarm.World.Spawners;
using System;
using System.Collections.Generic;

namespace StarSwarm.World.Spawners
{
    public class PlanetSpawner : Spawner
    {
        [Export]
        public PackedScene Planet { get; set; } = default!;
        [Export]
        public int MaxPlanets = 8;
        [Export]
        public int StartPlanets = 8;
        [Export]
        public float SpawnDistance = 800;

        private readonly List<Vector2> _availableSpawns = new List<Vector2>();
        private List<PackedScene> _weaponAttachments = default!;
        private List<PackedScene> _planetSkins = default!;
        private RandomNumberGenerator _rng = default!;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            _planetSkins = new List<PackedScene>() {
                GD.Load("res://Project/Planets/PixelPlanets/NoAtmosphere/NoAtmosphere.tscn") as PackedScene ?? throw new NullReferenceException(),
                GD.Load("res://Project/Planets/PixelPlanets/DryTerran/DryTerran.tscn") as PackedScene ?? throw new NullReferenceException(),
                GD.Load("res://Project/Planets/PixelPlanets/GasPlanet/GasPlanet.tscn") as PackedScene ?? throw new NullReferenceException(),
                GD.Load("res://Project/Planets/PixelPlanets/IceWorld/IceWorld.tscn") as PackedScene ?? throw new NullReferenceException(),
                GD.Load("res://Project/Planets/PixelPlanets/LandMasses/LandMasses.tscn") as PackedScene ?? throw new NullReferenceException(),
                GD.Load("res://Project/Planets/PixelPlanets/LavaWorld/LavaWorld.tscn") as PackedScene ?? throw new NullReferenceException(),
                GD.Load("res://Project/Planets/PixelPlanets/Rivers/Rivers.tscn") as PackedScene ?? throw new NullReferenceException(),
            };

            _weaponAttachments = new List<PackedScene>() {
                GD.Load("res://Project/Weapons/LightningRod/LightningRodAttachment.tscn") as PackedScene ?? throw new NullReferenceException(),
                GD.Load("res://Project/Weapons/LaserBeam/LaserBeamAttachment.tscn") as PackedScene ?? throw new NullReferenceException(),
                GD.Load("res://Project/Weapons/SpaceMine/SpaceMineAttachment.tscn") as PackedScene ?? throw new NullReferenceException(),
            };
        }

        public void Initialize(PlayerShip playerShip, RandomNumberGenerator rng)
        {
            _playerShip = playerShip;
            _rng = rng;
            CalculateSpawnPoints();
        }

        public void SpawnInitialPlanets()
        {
            for (var x = 0; x < StartPlanets; x++)
            {
                var spawnIndex = _rng.RandiRange(0, _availableSpawns.Count - 1);
                SpawnRandomPlanet(_availableSpawns[spawnIndex]);
                _availableSpawns.RemoveAt(spawnIndex);
            }
        }

        private void SpawnRandomPlanet(Vector2 position)
        {
            var spriteInstance = (Control)_planetSkins[_rng.RandiRange(0, _planetSkins.Count - 1)].Instance();
            var newPlanetInstance = (Planet)Planet.Instance();

            newPlanetInstance.Position = position + new Vector2(-50, -50);
            AddChild(newPlanetInstance);
            newPlanetInstance.AddChild(spriteInstance);

            var weaponAttachment = (WeaponAttachment)_weaponAttachments[_rng.RandiRange(0, _weaponAttachments.Count - 1)].Instance();

            newPlanetInstance.Initialize(spriteInstance, _playerShip, weapon: weaponAttachment);
        }

        private void CalculateSpawnPoints()
        {
            for (var x = 0; x < MaxPlanets; x++)
            {
                _availableSpawns.Add(new Vector2(
                    SpawnDistance * Mathf.Cos(2 * Mathf.Pi * x / MaxPlanets),
                    SpawnDistance * Mathf.Sin(2 * Mathf.Pi * x / MaxPlanets)
                ) + _playerShip.GlobalPosition);
            }
        }
    }
}
