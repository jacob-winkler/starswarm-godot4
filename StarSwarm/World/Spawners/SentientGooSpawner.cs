using Godot;
using StarSwarm.Ships.Enemies.SentientGoo;
using StarSwarm.World.Spawners;

namespace StarSwarm.StarSwarm.World.Spawners;
public partial class SentientGooSpawner : RadialSpawner<SentientGoo>
{
    public SentientGoo SpawnAtPosition(Vector2 position)
    {
        var instantiatedNode = (SentientGoo)NodeToSpawn.Instantiate();
        instantiatedNode.Position = position;
        AddChild(instantiatedNode);
        return instantiatedNode;
    }
}
