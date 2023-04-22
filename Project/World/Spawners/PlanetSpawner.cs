using Godot;
using StarSwarm.Project.Planets;
using StarSwarm.World.Spawners;
using System;
using System.Collections.Generic;

public class PlanetSpawner : Spawner
{
    [Export]
    public PackedScene Planet { get; set; } = default!;
    [Export]
    public int PlanetCount = 1;

    public List<PackedScene> PlanetSkins = default!;

    private RandomNumberGenerator _rng = default!;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        PlanetSkins = new List<PackedScene>() {
            GD.Load("res://Project/Planets/PixelPlanets/NoAtmosphere/NoAtmosphere.tscn") as PackedScene ?? throw new NullReferenceException(),
        };
    }

    public void Initialize(PlayerShip playerShip, RandomNumberGenerator rng)
    {
        _playerShip = playerShip;
        _rng = rng;
    }

    public void SpawnPlanets()
    {
        for (var x = 0; x < PlanetCount; x++)
            SpawnRandomPlanet(_playerShip.GlobalPosition + new Vector2(200, 200));
    }

    private void SpawnRandomPlanet(Vector2 position)
    {
        var spriteInstance = (Control)PlanetSkins[_rng.RandiRange(0, PlanetSkins.Count - 1)].Instance();
        var newPlanetInstance = (Planet)Planet.Instance();

        newPlanetInstance.Position = position;
        spriteInstance.RectPosition = newPlanetInstance.Position;

        newPlanetInstance.AddChild(spriteInstance);
        AddChild(newPlanetInstance);

        newPlanetInstance.Initialize(spriteInstance);
    }
}
