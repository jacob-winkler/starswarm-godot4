using StarSwarm.GSAI_Framework;
using StarSwarm.Ships.States;

namespace StarSwarm.Ships.Enemies.SpaceCrab.States;

public partial class SpaceCrabRest : Rest<SpaceCrab>
{
    private readonly GSAITargetAcceleration _acceleration = new GSAITargetAcceleration(); 

    public override void _PhysicsProcess(double delta)
    {
        Ship.Agent.ApplySteering(_acceleration, delta);
    }
}