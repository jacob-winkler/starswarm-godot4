using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using StarSwarm.World.Spawners;

namespace StarSwarm.World
{
    public class DifficultyScaler : Node
    {
        public Events Events { get; set; } = default!;
        public SpaceCrabSpawner SpaceCrabSpawner { get; set; } = default!;

        public override void _Ready()
        {
            SpaceCrabSpawner = GetNode<SpaceCrabSpawner>("../SpaceCrabSpawner");
            Events = GetNode<Events>("/root/Events");
            Events.Connect("GameMinutePassed", this, "OnMinutePassed");
        }

        public void OnMinutePassed()
        {
            SpaceCrabSpawner.MaxSpaceCrabs += 5;
            SpaceCrabSpawner.SpawnSpaceCrabs();
        }
    }
}