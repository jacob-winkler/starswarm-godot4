using StarSwarm.Project.GSAI_Framework;
using StarSwarm.Project.Ships.States;

namespace StarSwarm.Project.Ships.Enemies.SpaceCrab.States;

public partial class SpaceCrabRest : Rest<SpaceCrab>
{
    private GSAITargetAcceleration _acceleration = new GSAITargetAcceleration(); 

    public override void _PhysicsProcess(double delta)
    {
        Ship.Agent.ApplySteering(_acceleration, delta);
    }
}